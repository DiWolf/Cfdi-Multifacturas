using System;
using System.Collections.Generic;
using Multifacturas.SDK;

namespace MultiFacturasConsole
{
    public class Program
    {
        static CFDI factura;
        static SDK sdk;
        static PAC pac;
        static Conf conf;
        static SDKConfig sdkconf;

        //Emisor
        static Emisor emisor;
        static Receptor receptor;
        //Productos
        static Concepto concepto; 
        public static void Main(string[] args)
        {
            CrearFactura();
        }
        public static void CrearFactura()
        {
            //Aquí configuramos el rfc con el cliente, como vamos a hacer pruebas utilizaremos el rfc de pruebas
            // Para cambiar a produción simplemente haga cambie de NO a SI. cuando esta configurado en NO estamos en prueba
            pac = new PAC("DEMO700101XXX", "DEMO700101XXX", "NO");
            //Configuración de certificados.
            conf = new Conf(@"C:\multifacturas_sdk\pruebas\CSD01_AAA010101AAA.cer",
                @"C:\multifacturas_sdk\pruebas\CSD01_AAA010101AAA.key",
                "12345678a");
            //Configuramos el sdk para el timbrado
            sdkconf = new SDKConfig(pac, conf, @"C:\multifacturas_sdk\");
            sdk = new SDK(sdkconf);

            //Datos del emisor. 
            emisor = new Emisor();

            //Domicilio
            emisor.Nombre = "Soluciones Integrales en Tecnologías de la Información del Maya S.A de C.V";
            emisor.RFC = "AAA010101AAA";

            //Creamos el domicilio fiscal del emisor

            DomicilioFiscal domiciliofiscal = new DomicilioFiscal();
            domiciliofiscal.Calle = " 41";
            domiciliofiscal.NoExterior = "318";
            domiciliofiscal.Colonia = "Montealban";
            domiciliofiscal.CodigoPostal = "97114";
            domiciliofiscal.Localidad = "Mérida";
            domiciliofiscal.Municipio = "Mérida";
            domiciliofiscal.Pais = "México";
            domiciliofiscal.Estado = "Yucatán";

            


            //En caso de que la expedisión sea en otra sucursal.
            ExpedidoEn expedidoen = new ExpedidoEn();
            expedidoen.Calle = "Andador Potasio";
            expedidoen.NoExterior = "279";
            expedidoen.NoInterior = "3A";
            expedidoen.Colonia = "11 de Julio";
            expedidoen.CodigoPostal = "60954";
            expedidoen.Localidad = "Lázaro Cárdenas";
            expedidoen.Municipio = "Lázaro Cárdenas";
            expedidoen.Pais = "México";
            expedidoen.Estado = "Michoacán";

            emisor.Domicilio = domiciliofiscal;
            emisor.ExpedidoEn = expedidoen;

            ///Creamos el receptor
            receptor = new Receptor();
            receptor.Nombre = "Francisco Javier Guerrero";
            receptor.RFC = "XAXX010101000"; //aquí publico gneral. 

            Domicilio domicilioreceptor = new Domicilio();
            domicilioreceptor.Calle = "Domicilio Conocido";
            domicilioreceptor.Localidad = "Lazaro Cárdenas";


            receptor.Domicilio = domicilioreceptor;

            

            

            //No es necesario llenar todos los campos. 

            //Creamos la venta 
            List<Concepto> conceptos = new List<Concepto>();
            conceptos.Add(new Concepto("1", "Pieza", "COD1", "Producto Prueba", "120", "120"));
            Impuestos impuestos = new Impuestos();
            impuestos.AgregaTraslado(new Translado("IVA", "19.2", "16"));

            //Creamos un nuevo objeto de tipo factura


            factura = new CFDI();
            //Datos básicos de la factura
            //factura.Emisor = emisor;
            //factura.Receptor = receptor;
            factura.Emisor = emisor;
            factura.Receptor = receptor;
            factura.Conceptos = conceptos;
            factura.Impuestos = impuestos;
            //Configuramos la serie y el folio
            factura.Serie = "AA";
            factura.Folio = "1";
            factura.FechaDeExpedicion = "2016-12-02 19:00:00";
            //Consultar los cambios a la miscelanea fiscal Agosto 2016.
            factura.MetodoDePago = "01";
            //Los 4 digitos si pagó con tarjeta o cheque... 
            factura.NumCtaPAgo = "";
            //Esto se configura según las reglas del negocio;
            factura.FormaDePago = "Pago en una sola exhibición";
            //Peso mexicano... cambiar al tipo de cambio en caso de cobrar en USD o en otra moneda.
            factura.TipoDeCambio = "1.0";
            //Consultar el Anexo 20.
            factura.TipoDeComprobante = "ingreso"; //ingreso, egreso, traslado [Escribir en minúsculas]
            factura.LugarDeExpedicion = "Lázaro Cárdenas, Michoacan";
            factura.RegimenFiscal = "De las personas fisicas con actividad empresarial";
            factura.SubTotal = "120";
            factura.Descuento = "0";
            factura.Total = "139.2"; //El total con el iva NETO
            var x = AppDomain.CurrentDomain.BaseDirectory;
            sdk.CreaINI(factura, AppDomain.CurrentDomain.BaseDirectory + "Factura01"); //Esta cadena se cambia por la de tu archivo a generar.

            //Ahora creamos el tmbrado con el archivo INI ya generado (generamos el XML) 
            //factura, directorio y el archivo .ini generado
            sdk.Timbrar(factura,AppDomain.CurrentDomain.BaseDirectory,"Factura01");

        }

    }
}

    

