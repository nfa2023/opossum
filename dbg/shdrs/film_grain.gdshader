shader_type canvas_item;

// set max strength to your liking here=128.0, step_size=0.1
uniform float strength:hint_range(0.0, 1.0, 0.01)=0.2;
uniform float white_level:hint_range(0.01, 1.0, 0.01)=0.01;
uniform float min_grain:hint_range(0.0, 0.5, 0.01)=0.0;
uniform sampler2D BLUE_NOISE : repeat_enable, source_color;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

// Gets the luminance value.
//
// Returns [0 → 1] based on how close color is to [Black → White]
float Lumen(vec4 clr) { return 0.2126*clr.r + 0.7152*clr.g + 0.0722*clr.b; }

vec4 AddFilmGrain(in vec4 screen, in vec4 noise)
{
	float lumen = 1.0 - Lumen(screen); // calculate proximity to white
	noise.a = smoothstep(0.0, white_level, lumen + min_grain) * strength;
	return noise;
}

float random (vec2 st) 
{
    return fract(sin(dot(st.xy, vec2(12.9898,78.233)))*
        43758.5453123);
}

float InvLerp(float _max, float _min, float _val)
{
	return ( _val - _min ) / ( _max - _min );
}

void fragment() 
{
	vec2 _uv = UV;
	float offset = 0.5;
	float half_offset = offset * 0.5;
	
	_uv += offset;
	vec2 grain_speed = vec2(TIME * random(_uv) * 0.013);
	
	// TODO(RYAN_2023-11-13): 	Fix this to not require these if-statements.
	if(UV.x > 0.5) { grain_speed.x *= InvLerp(offset + 1.0, offset, _uv.x); }
	if(UV.y > 0.5) { grain_speed.y *= InvLerp(offset + 1.0, offset, _uv.y); }
	
	vec4 screen = texture(SCREEN_TEXTURE, SCREEN_UV);
	vec4 noise = texture(BLUE_NOISE, _uv * grain_speed);
	screen = AddFilmGrain(screen, noise);
	COLOR = screen;
}