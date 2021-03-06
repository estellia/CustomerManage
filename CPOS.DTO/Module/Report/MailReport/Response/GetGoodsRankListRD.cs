﻿using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Report.MailReport.Response
{
    public class GetGoodsRankListRD : IAPIResponseData
    {
        public GetGoodsRankListRD(List<R_WxO2OPanel_ItemTopTenEntity> dbList)
        {
            Top10Views = dbList.Where(x => x.ItemType == 10).OrderByDescending(x => x.ItemPV).Select(x =>
                new GoodsListInfo()
                {
                    ItemId = x.ItemID,
                    ItemName = x.ItemName,
                    ItemSoldCount = x.ItemSoldCount,
                    Rate = (x.ItemSoldCount != null && x.ItemUV != null && x.ItemUV != 0 ? Math.Round((decimal)x.ItemSoldCount * 100 / (decimal)x.ItemUV, 2,MidpointRounding.AwayFromZero).ToString() : "0") + "%"
                }).ToList();

            Least10Views = dbList.Where(x => x.ItemType == 30).OrderBy(x => x.ItemPV).Select(x =>
                new GoodsListInfo()
                {
                    ItemId = x.ItemID,
                    ItemName = x.ItemName,
                    ItemSoldCount = x.ItemSoldCount,
                    Rate = (x.ItemSoldCount != null && x.ItemUV != null && x.ItemUV != 0 ? Math.Round((decimal)x.ItemSoldCount * 100 / (decimal)x.ItemUV, 2, MidpointRounding.AwayFromZero).ToString() : "0") + "%"
                }).ToList();

            Top10Sales = dbList.Where(x => x.ItemType == 20).OrderByDescending(x => x.ItemSoldCount).Select(x =>
                new GoodsListInfo()
                {
                    ItemId = x.ItemID,
                    ItemName = x.ItemName,
                    ItemSoldCount = x.ItemSoldCount,
                    Rate = (x.ItemSoldCount != null && x.ItemUV != null && x.ItemUV != 0 ? Math.Round((decimal)x.ItemSoldCount * 100 / (decimal)x.ItemUV, 2, MidpointRounding.AwayFromZero).ToString() : "0") + "%"
                }).ToList();

            Least10Sales = dbList.Where(x => x.ItemType == 40).OrderBy(x => x.ItemSoldCount).Select(x =>
                new GoodsListInfo()
                {
                    ItemId = x.ItemID,
                    ItemName = x.ItemName,
                    ItemSoldCount = x.ItemSoldCount,
                    Rate = (x.ItemSoldCount != null && x.ItemUV != null && x.ItemUV != 0 ? Math.Round((decimal)x.ItemSoldCount * 100 / (decimal)x.ItemUV, 2, MidpointRounding.AwayFromZero).ToString() : "0") + "%"
                }).ToList();
        }
        public List<GoodsListInfo> Top10Views { get; set; }
        public List<GoodsListInfo> Least10Views { get; set; }
        public List<GoodsListInfo> Top10Sales { get; set; }
        public List<GoodsListInfo> Least10Sales { get; set; }
    }
    public class GoodsListInfo
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemSoldCount { get; set; }
        public string Rate { get; set; }
    }
}
