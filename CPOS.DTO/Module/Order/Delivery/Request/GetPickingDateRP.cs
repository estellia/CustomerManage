using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Order.Delivery.Request
{
    public class GetPickingDateRP : IAPIRequestParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public string DeliveryId { get; set; }

        public void Validate()
        {

        }
    }
}
