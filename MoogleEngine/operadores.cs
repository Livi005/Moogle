namespace MoogleEngine
{
    public class Operadores
    {
        public static bool[] Mascara(List<Dictionary<string, int>> textos, List<string> exc, List<string> pico)
        {
            //,mascara booleana para saber si ya escogi la palabra en el texto (true).
            bool[] b = new bool[textos.Count];

            // for para saber si la palabra esta en el texto (palabra q no debe estar en el texto)
            for (int j = 0; j < textos.Count; j++)
            {
                for (int i = 0; i < exc.Count; i++)
                {
                    //buscar para saber si la palabra tiene un operedor 
                    if (textos[j].ContainsKey(exc[i]))
                    {
                        b[j] = true;
                        break;
                    }
                }
            }

            bool esta;
            // for para saber si la palabra esta en el texto (palabra q debe estar en el texto)
            for (int j = 0; j < textos.Count; j++)
            {
                if (!b[j])
                {
                    for (int i = 0; i < pico.Count; i++)
                    {
                        if (!textos[j].ContainsKey(exc[i]))
                        {
                            b[j] = true;
                            break;
                        }
                    }
                }
            }

            return b;

        }

        // calculo (segun las cantidas de asteriscos) del valor de la palabra en el texto.
        public static int Multiplicar(List<(string, int)> aster, string query)
        {
            int numero = 0;
            for (int i = 0; i < aster.Count; i++)
            {
                if (aster[i].Item1 == query)
                    numero += aster[i].Item2;

            }
            return numero * 25;
        }

        // calculo de la menor distancia entre dos palabras en el texto.
        public static int Distancia(string palabra1, string palabra2, List<string> textos)
        {
            string pibote = palabra1;
            string buscar = palabra2;
            int menor_distancia = int.MaxValue;

            for (int i = 0; i < textos.Count; i++)
            {
                if (textos[i] == pibote)
                {
                    for (int j = i + 1; j < textos.Count; j++)
                    {
                        if (textos[j] == buscar)
                        {
                            int distancia = j - i;
                            if (distancia < menor_distancia)
                            {
                                menor_distancia = distancia;
                            }

                            string temp = pibote;
                            pibote = textos[j];
                            buscar = temp;
                            i = j;
                        }

                        if (textos[j] == pibote)
                            i = j;
                    }
                }

                if (textos[i] == buscar)
                {
                    for (int j = i + 1; j < textos.Count; j++)
                    {
                        if (textos[j] == pibote)
                        {
                            int distancia = j - i;
                            if (distancia < menor_distancia)
                            {
                                menor_distancia = distancia;
                            }

                            string temp = buscar;
                            buscar = textos[j];
                            pibote = temp;
                        }

                        if (textos[j] == buscar)
                            i = j;
                    }

                }
            }

            return menor_distancia;
        }

        //menor distancia de las palabras en cada texto.
        public static int[,] Menor_distancia(List<(string, string)> ene, List<List<string>> textos)
        {
            int[,] menor = new int[ene.Count, textos.Count];

            for (int i = 0; i < ene.Count; i++)
            {
                for (int j = 0; j < textos.Count; j++)
                {
                    menor[i, j] = Distancia(ene[i].Item1, ene[i].Item2, textos[j]);
                }
            }

            return menor;
        }
    }
}