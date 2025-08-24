using CapaDatos;
using SAPbobsCOM;
using System;
using System.Net;
using System.Runtime.Serialization.Json;
using static CapaEntidades.TipoCambio;

namespace CapaNegocios
{
    public class TipoCambioBLL
    {
        private static Company oCompany = new Company();
        private static string errorMessage;
        const string BANXICO_MI_TOKEN = "6c06b8cdbd6e0fd66b1d4f70f373402edc8dd31b51a9685a19dc5cc32bee634c";

        const string BANXICO_URL = "https://www.banxico.org.mx/SieAPIRest/service/v1/series/{0}/datos/{1}/{1}";
        const string BANXICO_FORMATO_FECHA = "yyyy-MM-dd";
        const string BANXICO_HEADER_ITEMTOKEN = "Bmx-Token";
        const string BANXICO_HEADER_FORMATACCEPTED = "application/json";
        //CODIGOS DE BANXICO PARA OBTENER TIPO DE CAMBIO
        const string BANXICO_SERIE_TIPOCAMBIOFIX = "SF60653";
        const string BANXICO_SERIE_TIPOCAMBIOCAN = "SF60632";
        const string BANXICO_SERIE_TIPOCAMBIOEUR = "SF46410";
        const string BANXICO_SERIE_TIPOCAMBIOGBP = "SF57815";

        static Response ReadSerie(string serie, DateTime fecha)
        {
            Response _result = null;
            string _strSerie = serie;
            string _fmtFecha = fecha.ToString(BANXICO_FORMATO_FECHA);

            try
            {
                string _url = string.Format(BANXICO_URL, _strSerie, _fmtFecha);

                HttpWebRequest _webRequest = WebRequest.Create(_url) as HttpWebRequest;
                _webRequest.Accept = BANXICO_HEADER_FORMATACCEPTED;
                _webRequest.Headers[BANXICO_HEADER_ITEMTOKEN] = BANXICO_MI_TOKEN;
                HttpWebResponse _webResponse = _webRequest.GetResponse() as HttpWebResponse;

                if (_webResponse.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    _webResponse.StatusCode,
                    _webResponse.StatusDescription));

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                object objResponse = jsonSerializer.ReadObject(_webResponse.GetResponseStream());
                _result = objResponse as Response;
            }
            catch (Exception e)
            {
            }

            return _result;
        }

        //USD
        public static string TipoDeCambioFIX(DateTime fecha)
        {
            string _result = string.Empty;
            try
            {
                Response _responce = ReadSerie(BANXICO_SERIE_TIPOCAMBIOFIX, fecha);
                if (_responce.seriesResponse.series[0].Data != null)
                {
                    if (_responce.seriesResponse.series[0].Data[0].Data == "N/E")
                    {
                        return TipoDeCambioFIX(fecha.AddDays(-1));
                    }
                    else
                    {
                        return _result = _responce.seriesResponse.series[0].Data[0].Data;
                    }
                }
                else
                {
                    return TipoDeCambioFIX(fecha.AddDays(-1));
                }
            }
            catch (Exception ex)
            {
                return TipoDeCambioFIX(fecha.AddDays(-1));
            }
        }

        //EUR
        public static string TipoDeCambioEUR(DateTime fecha)
        {
            string _result = string.Empty;
            try
            {
                Response _responce = ReadSerie(BANXICO_SERIE_TIPOCAMBIOEUR, fecha);

                if (_responce.seriesResponse.series[0].Data != null)
                {
                    if (_responce.seriesResponse.series[0].Data[0].Data == "N/E")
                    {
                        return TipoDeCambioEUR(fecha.AddDays(-1));
                    }
                    else
                    {
                        _result = _responce.seriesResponse.series[0].Data[0].Data;
                    }
                }
                else
                {
                    return TipoDeCambioEUR(fecha.AddDays(-1));
                }
            }
            catch (Exception ex)
            {
                return TipoDeCambioEUR(fecha.AddDays(-1));
            }

            return _result;
        }

        //CAN
        public static string TipoDeCambioCAN(DateTime fecha)
        {
            string _result = string.Empty;
            try
            {
                Response _responce = ReadSerie(BANXICO_SERIE_TIPOCAMBIOCAN, fecha);
                if (_responce.seriesResponse.series[0].Data != null)
                {
                    if (_responce.seriesResponse.series[0].Data[0].Data == "N/E")
                    {
                        return TipoDeCambioCAN(fecha.AddDays(-1));
                    }
                    else
                    {
                        _result = _responce.seriesResponse.series[0].Data[0].Data;
                    }
                }
                else
                {
                    return TipoDeCambioCAN(fecha.AddDays(-1));
                }
            }
            catch (Exception ex)
            {
                return TipoDeCambioCAN(fecha.AddDays(-1));
            }

            return _result;
        }

        //EUR
        public static string TipoDeCambioGBP(DateTime fecha)
        {
            string _result = string.Empty;
            try
            {
                Response _responce = ReadSerie(BANXICO_SERIE_TIPOCAMBIOGBP, fecha);

                if (_responce.seriesResponse.series[0].Data != null)
                {
                    if (_responce.seriesResponse.series[0].Data[0].Data == "N/E")
                    {
                        return TipoDeCambioGBP(fecha.AddDays(-1));
                    }
                    else
                    {
                        _result = _responce.seriesResponse.series[0].Data[0].Data;
                    }
                }
                else
                {
                    return TipoDeCambioGBP(fecha.AddDays(-1));
                }
            }
            catch (Exception ex)
            {
                return TipoDeCambioGBP(fecha.AddDays(-1));
            }

            return _result;
        }

        //CONETARSE PARA ACTUALIZAR EL T.C.
        public static bool setTipoCambio(String moneda, DateTime fecha, double tipodecambio)
        {
            try
            {
                bool resultado = false;
                SBObob oTSSL_bob;
                oTSSL_bob = DIAPIDAL.company.GetBusinessObject(BoObjectTypes.BoBridge);
                oTSSL_bob.SetCurrencyRate(moneda, fecha, tipodecambio, true);
                resultado = true;
                return resultado;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}