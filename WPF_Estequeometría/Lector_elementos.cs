using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WPF_Estequeometria
{
    class Lector_elementos
    {
        //primero leer el xml, ver cuántos elementos tiene, después inicializar el array
        private List<Elemento> metales = new List<Elemento>();

        public List<Elemento> leerXML(tipoElemento tipo) //"metal" o "noMetal"
        {
            List<Elemento> elementosABuscar = new List<Elemento>();

            XmlDocument xDoc = new XmlDocument();
            //XDocument xDoc = XDocument.Parse(Properties.Resources.elementos);

            //La ruta del documento XML permite rutas relativas 
            //respecto del ejecutable!
            XmlNodeList lista;
            XmlNodeList auxLista;

            xDoc.LoadXml(Properties.Resources.elementos);//"elementos.xml");

            if (tipo == tipoElemento.metal)
            {
                auxLista = xDoc.GetElementsByTagName("metales");

                lista = ((XmlElement)auxLista[0]).GetElementsByTagName("metal");
            } 
            else
            {
                auxLista = xDoc.GetElementsByTagName("no-metales");

                lista = ((XmlElement)auxLista[0]).GetElementsByTagName("nometal");
            }
            
            foreach (XmlElement nodo in lista)

            {

                int i = 0;

                XmlNodeList nNombre = nodo.GetElementsByTagName("nombre");

                XmlNodeList nSimbolo = nodo.GetElementsByTagName("simbolo");

                XmlNodeList nValencias = nodo.GetElementsByTagName("valencias");

                
                string nombreElemento = nNombre[i].InnerText;
                nombreElemento = nombreElemento.Substring(1, nombreElemento.Length - 2);
                string simboloElemento = nSimbolo[i].InnerText;
                simboloElemento = simboloElemento.Substring(1, simboloElemento.Length - 2);
                string[] auxValencias = nValencias[i++].InnerText.Split(new Char[] { '"', '\\' });
                List<string> valenciasElemento = new List<string>();
                foreach (string cadena in auxValencias)
                    if (cadena != "")
                    if (cadena != '"'.ToString())
                        if (cadena != '\\'.ToString())
                            valenciasElemento.Add(cadena);

                elementosABuscar.Add(new Elemento(nombreElemento, simboloElemento, valenciasElemento));

            }


            return elementosABuscar;

        }//Fin de metdo leerXML.
    }

}
