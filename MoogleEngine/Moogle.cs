namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        List<string> quary = Normalizacion.Hazlo_todo(query); //normalizacion d la query
        List<(string, float)> quary1 = Mi_Query.Repeticion_de_palabras(quary);// las palabras d la query con la cant d veces que se repiten

        (List<string>, List<string>) txt = Procesamiento_txt.Leer();
        List<List<string>> txt1 = Procesamiento_txt.Procesar(txt.Item2);//procesamiento de los textos

        List<Dictionary<string, int>> txt2 = Procesamiento_txt.CreateDictionary(txt1);//procesamiento de los textos

        List<string> semejantes = Sugerencias.Palabras_semejantes(quary1, txt2); //palabras semejantes (levenstain)

        //Aqui se van a guardar:
        List<string> exc;                    //palabras con el operador de exclaacion delante.
        List<string> pico;                  //palabras con el operador pico delante.
        List<(string, int)> aster;         //palabras con el operador asterisco delante y la cantidad que tienen.
        List<(string, string)> ene;       //palabras con el operador de la tilde de la 'n' entre ellas.

        (exc, pico, aster, ene) = Mi_Query.Operadores(query); 

        bool[] b = Operadores.Mascara(txt2, exc, pico);

        float[] f;
        float[] idf;
        (f, idf) = Tf_Idf.Resultado(txt2, quary1, aster, ene, b, txt1);  //calculo del score segun los operadores de la query

        Tf_Idf.Ordenar(txt.Item1, txt.Item2, f);  //ordenar los textos segun el tamanno del score.


        SearchItem[] items = Moogle.Resultado(txt, quary, f);
        return new SearchResult(items, Sugerencias.Sugerir(quary1, idf, semejantes, txt1.Count - 1));
    }

    //el resultado de la busqueda (titulo, texto, valor del Tf_IDF).
    public static SearchItem[] Resultado((List<string>, List<string>) txt, List<string> quary, float[] Tf_IDF)
    {
        SearchItem busqueda;
        List<SearchItem> resultado = new List<SearchItem>(); //cantidad de textos para poner en el resultado.

        for (int i = 0; i < 5 && i < txt.Item1.Count; i++)
        {
            if (Tf_IDF[i] <= 0) break;

            busqueda = new SearchItem(txt.Item1[i], Procesamiento_txt.Recorte_de_textos(txt.Item2, Tf_IDF, quary, txt.Item2[i]), Tf_IDF[i]);
            resultado.Add(busqueda);
        }
        return resultado.ToArray();
    }
}
