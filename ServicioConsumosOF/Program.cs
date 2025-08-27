using CapaNegocios;

namespace ServicioConsumosOF
{
    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            {
                bool bandera = true;
                //if (DateTime.Now.Minute % 1 == 0 && bandera == true && DateTime.Now.Second > 15 && DateTime.Now.Second < 35)
                {
                    OrdenFabricacionBLL ordenFabricacionBLL = new OrdenFabricacionBLL();
                    ////ordenFabricacionBLL.crearConsumoAutomatico();
                    ordenFabricacionBLL.crearConsumoAutomaticoROVE();
                    ordenFabricacionBLL.crearGeneracionAutomaticoROVE();                    
                }
            }
        }
    }
}