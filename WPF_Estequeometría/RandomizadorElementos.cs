using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class RandomizadorElementos
    {
        ListaMetales listamet = new ListaMetales();
        ListaNoMetales listanomet = new ListaNoMetales();
        List<Elemento> listaUsables = new List<Elemento>();
        tipoMolécula tipo;
        //ListaMetales listaMetYaUsados = new ListaMetales();
        //ListaNoMetales listaNoMetYaUsados = new ListaNoMetales();

        public RandomizadorElementos(tipoMolécula t)
        {
            tipo = t;
            cargarElementosEnUsables(t);
        }

        private void cargarElementosEnUsables(tipoMolécula t)
        {
            if (t != tipoMolécula.acido) //los metales se cargan en todos los tipos de mol menos en ácidos
            {
                foreach (Elemento el in listamet.Metales)
                    listaUsables.Add(el);
            }

            if (t != tipoMolécula.hidroxido) //los no metales se cargan en todos los tipos de mol menos en hidróxidos
            {
                foreach (Elemento el in listanomet.Nometales)
                    listaUsables.Add(el);
            }
        }

        public ElementoEnUso elegirAtomoAleatorio()
        {

            Random rnd = new Random();
            int cantElementos = listaUsables.Count;
            
            Elemento el;
            int valenciaAleatoria = 1;

            int i = rnd.Next(cantElementos-1);

            el = listaUsables[i];

            i = rnd.Next(el.Valencias.Count);
            valenciaAleatoria = int.Parse(el.Valencias[i]);

            ElementoEnUso ele = new ElementoEnUso(el.Nombre, el.Simbolo, valenciaAleatoria, true); //se inicializa por las dudas
            
            listaUsables.Remove(el); //se quita de usables el elemento que se va a devolver
            if (listaUsables.Count <= 0) 
                cargarElementosEnUsables(tipo); //si salieron todos los elementos, se empieza de nuevo

            return ele;
        }


        ////public ElementoEnUso elegirAtomoAleatorio()
        ////{
            
        ////    Random rnd = new Random();
        ////    int cantMetales = listamet.Metales.Count;
        ////    int cantNoMetales = listanomet.Nometales.Count;
        ////    Elemento el;
        ////    int valenciaAleatoria = 1;

        ////    int i = rnd.Next(cantMetales + cantNoMetales);

        ////    if (i >= cantMetales) //como se suman las listas de metales y no metales, si el número es superior al total de metales, quiere decir que es no metal
        ////    {
        ////        i -= cantMetales; //se resta la lista de los metales
        ////        el = listanomet.Nometales[i];
        ////    }
        ////    else
        ////    {
        ////        el = listamet.Metales[i];
        ////    }

        ////    i = rnd.Next(el.Valencias.Count);
        ////    valenciaAleatoria = int.Parse(el.Valencias[i]);

        ////    //List<string> valencias = new List<string>();
        ////    //valencias.Add(valenciaAleatoria.ToString());

        ////    //Elemento elemento = new Elemento(el.Nombre, el.Simbolo, valencias);
        ////    ElementoEnUso ele = new ElementoEnUso(el.Nombre, el.Simbolo, valenciaAleatoria, true); //se inicializa por las dudas



        ////    //if (esMetalYaUsado(ele) || esNoMetalYaUsado(ele)) //se chequea si ya se usó
        ////    //    elegirAtomoAleatorio();
        ////    //else
        ////    //    ele = new ElementoEnUso(el.Nombre, el.Simbolo, valenciaAleatoria, true);

        ////    //if (listaMetYaUsados.Metales.Count >= listamet.Metales.Count) listaMetYaUsados.Metales.Clear(); //si salieron todos los metales, se empieza de nuevo
        ////    //if (listaNoMetYaUsados.Nometales.Count >= listanomet.Nometales.Count) listaNoMetYaUsados.Nometales.Clear(); //si salieron todos los metales, se empieza de nuevo

        ////    //if (!esMetalYaUsado(ele) && esMetal(ele)) //no debería ser necesario chequear pero por las dudas lo hacemos
        ////    //    listaMetYaUsados.Metales.Add(elemento);//se añade el elemento que se va a devolver en la función

        ////    //if (!esNoMetalYaUsado(ele) && esNoMetal(ele)) //no debería ser necesario chequear pero por las dudas lo hacemos
        ////    //    listaNoMetYaUsados.Nometales.Add(elemento);//se añade el elemento que se va a devolver en la función

        ////    if (esYaUsado(ele)) //se chequea si ya se usó
        ////        elegirAtomoAleatorio();
        ////    //else
        ////        //ele = new ElementoEnUso(el.Nombre, el.Simbolo, valenciaAleatoria, true);

        ////    if (yaUsados.Count >= listamet.Metales.Count + listanomet.Nometales.Count) yaUsados.Clear(); //si salieron todos los elementos, se empieza de nuevo

        ////    if (!esYaUsado(ele)) //no se debería chequear, pero por las dudas
        ////        yaUsados.Add(ele); //se añade el elemento que se va a devolver

        ////    return ele;

        ////}

        ////private bool esMetalYaUsado (ElementoEnUso elementoParaChequear)
        ////{
        ////    foreach (Elemento el in listaMetYaUsados.Metales)
        ////    {
        ////        if (elementoParaChequear.Simbolo == el.Simbolo)
        ////            return true;
        ////    }

        ////    return false;
        ////}

        ////private bool esNoMetalYaUsado(ElementoEnUso elementoParaChequear)
        ////{
        ////    foreach (Elemento el in listaNoMetYaUsados.Nometales)
        ////    {
        ////        if (elementoParaChequear.Simbolo == el.Simbolo)
        ////            return true;
        ////    }

        ////    return false;
        ////}

        //private bool esYaUsado(ElementoEnUso elementoParaChequear)
        //{
        //    foreach (ElementoEnUso el in yaUsados)
        //    {
        //        if (elementoParaChequear.Simbolo == el.Simbolo)
        //            return true;
        //    }

        //    return false;
        //}

        //private bool esMetal(ElementoEnUso elementoParaChequear)
        //{
        //    foreach (Elemento el in listamet.Metales)
        //    {
        //        if (elementoParaChequear.Simbolo == el.Simbolo)
        //            return true;
        //    }

        //    return false;
        //}

        //private bool esNoMetal(ElementoEnUso elementoParaChequear)
        //{
        //    foreach (Elemento el in listanomet.Nometales)
        //    {
        //        if (elementoParaChequear.Simbolo == el.Simbolo)
        //            return true;
        //    }

        //    return false;
        //}
    }
}
