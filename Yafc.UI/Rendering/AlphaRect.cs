using SDL2;

namespace Yafc.UI;

/// <summary>Draw a solid rectangle with optionally configurable transparency.</summary>
/// <remarks>
/// Due to <see cref="ImGui"/>'s bucketed rendering, <c>AlphaRect</c> renderables will be drawn at a z-index higher than
/// that of standard <see cref="ImGui"/> rectangles.
/// </remarks>
public class AlphaRect(byte alpha = 255) : IRenderable {
    public void Render(DrawingSurface surface, SDL.SDL_Rect position, SDL.SDL_Color color) {
        SDL.SDL_SetRenderDrawBlendMode(surface.renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
        SDL.SDL_SetRenderDrawColor(surface.renderer, color.r, color.g, color.b, (byte)(color.a * alpha / 255));
        SDL.SDL_RenderFillRect(surface.renderer, ref position);
    }
}
