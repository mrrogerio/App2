using App2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Utils
{
    public static class GlobalVariables
    {
        public static LoginFuncModel GlobalFuncionarioLogado { get; set; }
        public static ClienteModel GlobalClientePedido { get; set; }

        public static PedidoModel GlobalPedido { get; set; }

        public static int campanha = 13;
        public static int formaPagamento = 0;
        public static double percDesconto = 0;
    }
}
