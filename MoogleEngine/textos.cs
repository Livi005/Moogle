namespace MoogleEngine
{
    public static class Normalizacion //clase de normalizacion de textos en general.
    {
        static string Normalizar(string textos) //quitar mayusculas y tildes de textos.
        {
            string texto1 = textos.ToLower();
            texto1 = texto1.Replace('á', 'a');
            texto1 = texto1.Replace('é', 'e');
            texto1 = texto1.Replace('í', 'i');
            texto1 = texto1.Replace('ó', 'o');
            texto1 = texto1.Replace('ú', 'u');
            return texto1;
        }

        public static string[] Separar(string texto1) // separar las palabras de textos.
        {
            char[] separador = { '.', ',', ';', '(', ')', '[', ']', '\n', '\r', ':', ' ', '*', '!', '^', '~', '\t', '\\' };

            return texto1.Split(separador);
        }

        public static List<string> Hazlo_todo(string texto) // normalizacion y separacion de textos.
        {
            string texto1 = Normalizar(texto);
            List<string> textos = Separar(texto1).ToList();

            return textos;
        }

    }
    //Implementa de los metodos anteriores pero ya en los textos en especifico.
    class Procesamiento_txt // Procesamiento de los txt. 
    {
        public static (List<string>, List<string>) Leer() // Leer los txt y los titulos que tengo en la PC.
        {
            string[] directorio = Directory.GetFiles("../contents");
            string[] textos = new string[directorio.Length];
            string[] titulos = new string[directorio.Length];
            for (int i = 0; i < textos.Length; i++)
            {
                StreamReader leer = new StreamReader(directorio[i], System.Text.Encoding.UTF7);
                textos[i] = leer.ReadToEnd();// leer textos.
                titulos[i] = Path.GetFileName(directorio[i]);//leer titulos.
            }
            return (titulos.ToList(), textos.ToList());
        }
        public static List<Dictionary<string, int>> CreateDictionary(List<List<string>> textos)
        {
            List<Dictionary<string, int>> result = new List<Dictionary<string, int>>();
            for (int i = 0; i < textos.Count; i++)
            {
                int val = 0;
                result.Add(new Dictionary<string, int>());
                for (int j = 0; j < textos[i].Count; j++)
                {
                    if (result[i].TryGetValue(textos[i][j], out val))
                        result[i][textos[i][j]] = val + 1;
                    else
                        result[i].Add(textos[i][j], 1);
                }
            }
            return result;
        }
        // Llamar a los metodos de normalizacion de los txt que estan en la clase anterior. 
        public static List<List<string>> Procesar(List<string> textos)
        {
            List<List<string>> lista = new List<List<string>>();

            for (int i = 0; i < textos.Count; i++)
            {
                lista.Add(Normalizacion.Hazlo_todo(textos[i]));
            }

            return lista;
        }

        //Cantidad de palabras(25) que saldran despues de la busqueda, en los textos de resultado.(Snippet). el tf idf me da en que textos estan las palabras lo que me ayuda a saber la posicion.
        public static string Recorte_de_textos(List<string> textos, float[] Tf_Idf, List<string> query, string texto)
        {
            int i;

            float f = int.MinValue;
            string palabra = "";

            string recorte = "";
            string[] txt = Normalizacion.Separar(texto);
            List<string> txt1 = Normalizacion.Hazlo_todo(texto);

            for (i = 0; i < query.Count; i++)
            {
                if (Tf_Idf[i] > f)
                {
                    f = Tf_Idf[i];
                    palabra = query[i];
                }
            }

            int posicion = txt1.IndexOf(palabra);

            for (i = Math.Max(0, posicion - 33); i < Math.Min(posicion + 33, txt.Length); i++)
            {
                if (posicion != -1)
                {
                    recorte += txt[i] + " ";
                }
            }
            return recorte;
        }
    }
}


