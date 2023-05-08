namespace MoogleEngine
{
    public static class Sugerencias
    {
        //calcular distancia de Levenshtein
        public static int Levenshtein(string a, string b)
        {
            int i = a.Length - 1;
            int f = b.Length - 1;
            int value = int.MaxValue;
            Menor(i, f, a, b, ref value, 0);
            return value;
        }
        //cantidad de cambios q hay que hacer para convertir una palabra en otra.
        private static void Menor(int i, int f, string a, string b, ref int distancia, int current)
        {
            if (current < distancia && current < a.Length / 2)
            {
                if (i < 0) { distancia = Math.Min(f + 1 + current, distancia); return; }
                if (f < 0) { distancia = Math.Min(i + 1 + current, distancia); return; }

                if (a[i] == b[f]) { Menor(i - 1, f - 1, a, b, ref distancia, current); return; }

                current++;
                Menor(i - 1, f, a, b, ref distancia, current);
                Menor(i, f - 1, a, b, ref distancia, current);
                Menor(i - 1, f - 1, a, b, ref distancia, current);
            }
        }

        //palabras semejantas en los textos a las palabras de la query.
        public static List<string> Palabras_semejantes(List<(string, float)> query, List<Dictionary<string, int>> textos)
        {
            List<string> semejante = new List<string>();
            string palabra = "";
            int cambios = int.MaxValue;

            for (int i = 0; i < query.Count; i++)
            {
                for (int j = 0; j < textos.Count; j++)
                {
                    foreach (var key in textos[j].Keys)
                    {
                        int c = Levenshtein(query[i].Item1, key);

                        if (c < cambios && c > 0)
                        {
                            cambios = c;
                            palabra = key;
                        }
                    }
                }
                semejante.Add(palabra);
                cambios = int.MaxValue;
                palabra = "";
            }

            return semejante;
        }

        public static string Sugerir(List<(string, float)> query, float[] IDF, List<string> Palabras_semejantes, int n)
        {
            string sugerir = "";
            for (int i = 0; i < query.Count; i++)
            {
                if (IDF[i] >= 0.30)
                {
                    sugerir += Palabras_semejantes[i] + " ";
                }
                else
                {
                    sugerir += query[i].Item1 + " ";
                }
            }

            return sugerir;
        }
    }
}