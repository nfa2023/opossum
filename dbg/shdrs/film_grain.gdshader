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

vec4 AddFilmGrain(inout vec4 screen, in vec4 noise)
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

void fragment() 
{
	vec4 screen = texture(SCREEN_TEXTURE, SCREEN_UV);
	float grain_speed = TIME * random(UV) * 0.013;
	vec4 noise = texture(BLUE_NOISE, UV * grain_speed);
	screen = AddFilmGrain(screen, noise);
	COLOR = screen;
}