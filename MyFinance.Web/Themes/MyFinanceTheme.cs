using MudBlazor;

namespace MyFinance.Web.Themes
{
    public static class MyFinanceTheme
    {
        public static MudTheme DefaultTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                // Azul Profundo (Confiança/Institucional) - Cor principal
                Primary = "#0F172A",

                // Verde Esmeralda (Sucesso/Dinheiro)
                Secondary = "#10B981",

                // Um cinza azulado para o fundo do app (não branco puro, pra não cansar a vista)
                Background = "#F1F5F9",

                // Cor da Barra Lateral (Drawer) - Escura para dar contraste
                DrawerBackground = "#0F172A",
                DrawerText = "#FFFFFF",
                DrawerIcon = "#94A3B8", // Ícones levemente cinzas

                // Isso garante que os itens não selecionados fiquem bem brancos/claros
                ActionDefault = "#CBD5E1",

                // Cor do Topo (AppBar) - Branco clean
                AppbarBackground = "#FFFFFF",
                AppbarText = "#1E293B",

                // Cores de feedback
                Success = "#10B981",
                Error = "#EF4444",
                Warning = "#F59E0B",
                Info = "#3B82F6",

                // Cor de texto
                TextPrimary = "#1E293B",
                TextSecondary = "#64748B"
            },

            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = new[] { "Inter", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = 400,
                    LineHeight = 1.5,
                    LetterSpacing = ".00938em"
                },
                H1 = new H1 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 700 },
                H2 = new H2 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 700 },
                H3 = new H3 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 600 },
                H4 = new H4 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 600 },
                H5 = new H5 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 600 },
                H6 = new H6 { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 600 },
                Button = new Button { FontFamily = new[] { "Inter", "sans-serif" }, FontWeight = 500, TextTransform = "none" } // TextTransform none tira o CAPS LOCK automático
            },

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "8px", // Bordas arredondadas (estilo moderno)
                DrawerWidthLeft = "260px"    // Menu lateral um pouco mais largo
            }
        };
    }
}