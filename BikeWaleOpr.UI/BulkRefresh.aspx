<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Debug="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Memcache Check Page</title>
</head>
<body>
<form id="form1" runat="server">        
        Memcache Key :<asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
        <asp:Button ID="btnGetVal" Text="Get Memcache Object" runat="server" OnClick="btnGetVal_click" />
		<asp:CheckBox id="chkClearCache" runat="server"
                    AutoPostBack="false"
                    Text="Clear memcache object"
                    TextAlign="Right" />
    </form>
    <script runat="server">
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {  
            HttpContext.Current.Response.Write(BikeWaleOpr.Common.CurrentUser.Id); 			
        }
        
        protected void btnGetVal_click(object sender, EventArgs e)
        {
            string keyName = "";
            keyName = txtKey.Text;
			
            try{
			if (!String.IsNullOrEmpty(keyName))
	        {
		        Enyim.Caching.MemcachedClient _mc1= new Enyim.Caching.MemcachedClient("memcached");
                      
                        /*string key1 = keyName;
string[] keys =
{"BW_DealerBikeModel_21151_12",
"BW_DealerBikeModel_15370_17",
"BW_DealerBikeModel_21242_9",
"BW_DealerBikeModel_21242_17",
"BW_DealerBikeModel_15372_17",
"BW_DealerBikeModel_21243_1",
"BW_DealerBikeModel_14764_17",
"BW_DealerBikeModel_21256_13",
"BW_DealerBikeModel_20715_12",
"BW_DealerBikeModel_10243_9",
"BW_DealerBikeModel_10245_6",
"BW_DealerBikeModel_10248_61",
"BW_DealerBikeModel_10249_56",
"BW_DealerBikeModel_10275_13",
"BW_DealerBikeModel_10277_13",
"BW_DealerBikeModel_10292_7",
"BW_DealerBikeModel_10293_7",
"BW_DealerBikeModel_10308_7",
"BW_DealerBikeModel_10333_7",
"BW_DealerBikeModel_10551_7",
"BW_DealerBikeModel_10671_10",
"BW_DealerBikeModel_10674_10",
"BW_DealerBikeModel_10855_13",
"BW_DealerBikeModel_10863_7",
"BW_DealerBikeModel_11042_10",
"BW_DealerBikeModel_13262_5",
"BW_DealerBikeModel_11402_6",
"BW_DealerBikeModel_21348_16",
"BW_DealerBikeModel_10249_11",
"BW_DealerBikeModel_11888_16",
"BW_DealerBikeModel_11904_16",
"BW_DealerBikeModel_11954_16",
"BW_DealerBikeModel_11972_1",
"BW_DealerBikeModel_11974_7",
"BW_DealerBikeModel_11975_7",
"BW_DealerBikeModel_11977_7",
"BW_DealerBikeModel_11988_7",
"BW_DealerBikeModel_12444_10",
"BW_DealerBikeModel_12521_11",
"BW_DealerBikeModel_12539_15",
"BW_DealerBikeModel_12626_12",
"BW_DealerBikeModel_12627_12",
"BW_DealerBikeModel_12628_6",
"BW_DealerBikeModel_12667_12",
"BW_DealerBikeModel_12668_12",
"BW_DealerBikeModel_12670_12",
"BW_DealerBikeModel_12673_12",
"BW_DealerBikeModel_12718_9",
"BW_DealerBikeModel_12734_11",
"BW_DealerBikeModel_12759_10",
"BW_DealerBikeModel_12863_11",
"BW_DealerBikeModel_12879_12",
"BW_DealerBikeModel_12881_11",
"BW_DealerBikeModel_13127_11",
"BW_DealerBikeModel_13130_7",
"BW_DealerBikeModel_13176_7",
"BW_DealerBikeModel_13261_7",
"BW_DealerBikeModel_13294_11",
"BW_DealerBikeModel_13297_22",
"BW_DealerBikeModel_13328_10",
"BW_DealerBikeModel_13336_13",
"BW_DealerBikeModel_13876_1",
"BW_DealerBikeModel_13885_1",
"BW_DealerBikeModel_14239_7",
"BW_DealerBikeModel_14240_7",
"BW_DealerBikeModel_14374_11",
"BW_DealerBikeModel_14389_12",
"BW_DealerBikeModel_14392_16",
"BW_DealerBikeModel_14407_6",
"BW_DealerBikeModel_14408_13",
"BW_DealerBikeModel_14412_9",
"BW_DealerBikeModel_14414_1",
"BW_DealerBikeModel_14462_9",
"BW_DealerBikeModel_14488_13",
"BW_DealerBikeModel_14489_7",
"BW_DealerBikeModel_14490_7",
"BW_DealerBikeModel_14491_7",
"BW_DealerBikeModel_14494_11",
"BW_DealerBikeModel_14496_15",
"BW_DealerBikeModel_14515_12",
"BW_DealerBikeModel_14541_12",
"BW_DealerBikeModel_14567_1",
"BW_DealerBikeModel_14602_9",
"BW_DealerBikeModel_14607_17",
"BW_DealerBikeModel_14637_15",
"BW_DealerBikeModel_14641_15",
"BW_DealerBikeModel_14666_13",
"BW_DealerBikeModel_14667_13",
"BW_DealerBikeModel_14668_13",
"BW_DealerBikeModel_14677_13",
"BW_DealerBikeModel_14678_12",
"BW_DealerBikeModel_14679_12",
"BW_DealerBikeModel_14680_10",
"BW_DealerBikeModel_14712_7",
"BW_DealerBikeModel_14718_7",
"BW_DealerBikeModel_14719_12",
"BW_DealerBikeModel_14735_1",
"BW_DealerBikeModel_14737_1",
"BW_DealerBikeModel_14750_1",
"BW_DealerBikeModel_14755_1",
"BW_DealerBikeModel_14758_13",
"BW_DealerBikeModel_14760_9",
"BW_DealerBikeModel_14838_11",
"BW_DealerBikeModel_14882_1",
"BW_DealerBikeModel_14900_15",
"BW_DealerBikeModel_14911_12",
"BW_DealerBikeModel_14953_11",
"BW_DealerBikeModel_15004_13",
"BW_DealerBikeModel_15009_15",
"BW_DealerBikeModel_15023_8",
"BW_DealerBikeModel_15044_15",
"BW_DealerBikeModel_15240_6",
"BW_DealerBikeModel_15262_6",
"BW_DealerBikeModel_15353_1",
"BW_DealerBikeModel_15358_1",
"BW_DealerBikeModel_15588_6",
"BW_DealerBikeModel_15775_13",
"BW_DealerBikeModel_15776_13",
"BW_DealerBikeModel_15805_7",
"BW_DealerBikeModel_15812_16",
"BW_DealerBikeModel_15904_13",
"BW_DealerBikeModel_15915_12",
"BW_DealerBikeModel_15925_1",
"BW_DealerBikeModel_15926_9",
"BW_DealerBikeModel_10549_7",
"BW_DealerBikeModel_21434_6",
"BW_DealerBikeModel_12630_6",
"BW_DealerBikeModel_12633_12",
"BW_DealerBikeModel_12634_12",
"BW_DealerBikeModel_12864_7",
"BW_DealerBikeModel_12865_13",
"BW_DealerBikeModel_12867_15",
"BW_DealerBikeModel_12868_15",
"BW_DealerBikeModel_12869_15",
"BW_DealerBikeModel_12870_13",
"BW_DealerBikeModel_12872_13",
"BW_DealerBikeModel_13132_7",
"BW_DealerBikeModel_13894_1",
"BW_DealerBikeModel_14440_16",
"BW_DealerBikeModel_14467_9",
"BW_DealerBikeModel_14482_7",
"BW_DealerBikeModel_14615_7",
"BW_DealerBikeModel_14622_7",
"BW_DealerBikeModel_14647_1",
"BW_DealerBikeModel_14715_7",
"BW_DealerBikeModel_14741_1",
"BW_DealerBikeModel_14764_9",
"BW_DealerBikeModel_15063_8",
"BW_DealerBikeModel_15370_9",
"BW_DealerBikeModel_15372_9",
"BW_DealerBikeModel_15436_6",
"BW_DealerBikeModel_15593_11",
"BW_DealerBikeModel_15715_12",
"BW_DealerBikeModel_15803_7",
"BW_DealerBikeModel_15908_12",
"BW_DealerBikeModel_15937_6",
"BW_DealerBikeModel_16117_40",
"BW_DealerBikeModel_16157_13",
"BW_DealerBikeModel_16201_7",
"BW_DealerBikeModel_16203_7",
"BW_DealerBikeModel_16338_15",
"BW_DealerBikeModel_16344_15",
"BW_DealerBikeModel_16386_15",
"BW_DealerBikeModel_16404_15",
"BW_DealerBikeModel_16465_1",
"BW_DealerBikeModel_16573_15",
"BW_DealerBikeModel_10255_13",
"BW_DealerBikeModel_10273_13",
"BW_DealerBikeModel_10280_16",
"BW_DealerBikeModel_10530_7",
"BW_DealerBikeModel_11352_7",
"BW_DealerBikeModel_10211_1",
"BW_DealerBikeModel_12720_9",
"BW_DealerBikeModel_13129_11",
"BW_DealerBikeModel_13177_7",
"BW_DealerBikeModel_13263_12",
"BW_DealerBikeModel_13403_12",
"BW_DealerBikeModel_14410_9",
"BW_DealerBikeModel_14478_7",
"BW_DealerBikeModel_14492_6",
"BW_DealerBikeModel_14493_6",
"BW_DealerBikeModel_14537_12",
"BW_DealerBikeModel_14563_1",
"BW_DealerBikeModel_14566_1",
"BW_DealerBikeModel_14618_7",
"BW_DealerBikeModel_14742_1",
"BW_DealerBikeModel_14763_9",
"BW_DealerBikeModel_14880_1",
"BW_DealerBikeModel_14886_9",
"BW_DealerBikeModel_15592_11",
"BW_DealerBikeModel_15978_9",
"BW_DealerBikeModel_10281_16",
"BW_DealerBikeModel_10672_10",
"BW_DealerBikeModel_12632_13",
"BW_DealerBikeModel_12671_12",
"BW_DealerBikeModel_12871_13",
"BW_DealerBikeModel_14405_13",
"BW_DealerBikeModel_14606_9",
"BW_DealerBikeModel_14614_7",
"BW_DealerBikeModel_14648_1",
"BW_DealerBikeModel_14714_7",
"BW_DealerBikeModel_14753_1",
"BW_DealerBikeModel_14881_1",
"BW_DealerBikeModel_15241_6",
"BW_DealerBikeModel_15716_7",
"BW_DealerBikeModel_15916_12",
"BW_DealerBikeModel_10246_65",
"BW_DealerBikeModel_10287_22",
"BW_DealerBikeModel_11356_10",
"BW_DealerBikeModel_12546_13",
"BW_DealerBikeModel_12762_10",
"BW_DealerBikeModel_13151_1",
"BW_DealerBikeModel_13887_1",
"BW_DealerBikeModel_14105_6",
"BW_DealerBikeModel_14375_11",
"BW_DealerBikeModel_14388_12",
"BW_DealerBikeModel_14393_16",
"BW_DealerBikeModel_14419_13",
"BW_DealerBikeModel_14477_7",
"BW_DealerBikeModel_14560_1",
"BW_DealerBikeModel_14665_7",
"BW_DealerBikeModel_14759_9",
"BW_DealerBikeModel_14808_13",
"BW_DealerBikeModel_14870_11",
"BW_DealerBikeModel_15036_15",
"BW_DealerBikeModel_15804_7",
"BW_DealerBikeModel_15935_7",
"BW_DealerBikeModel_16463_1",
"BW_DealerBikeModel_12760_10",
"BW_DealerBikeModel_12761_10",
"BW_DealerBikeModel_13212_6",
"BW_DealerBikeModel_14391_8",
"BW_DealerBikeModel_14424_40",
"BW_DealerBikeModel_14565_1",
"BW_DealerBikeModel_14681_10",
"BW_DealerBikeModel_14762_9",
"BW_DealerBikeModel_15928_7",
"BW_DealerBikeModel_10288_7",
"BW_DealerBikeModel_10853_13",
"BW_DealerBikeModel_10211_58",
"BW_DealerBikeModel_10856_13",
"BW_DealerBikeModel_11353_7",
"BW_DealerBikeModel_11403_6",
"BW_DealerBikeModel_11993_7",
"BW_DealerBikeModel_12445_10",
"BW_DealerBikeModel_12528_6",
"BW_DealerBikeModel_12529_6",
"BW_DealerBikeModel_12530_6",
"BW_DealerBikeModel_12531_6",
"BW_DealerBikeModel_12532_6",
"BW_DealerBikeModel_12533_6",
"BW_DealerBikeModel_12534_6",
"BW_DealerBikeModel_12625_12",
"BW_DealerBikeModel_12672_12",
"BW_DealerBikeModel_12866_12",
"BW_DealerBikeModel_12882_11",
"BW_DealerBikeModel_13124_12",
"BW_DealerBikeModel_13128_11",
"BW_DealerBikeModel_13131_7",
"BW_DealerBikeModel_13148_12",
"BW_DealerBikeModel_13259_7",
"BW_DealerBikeModel_13260_7",
"BW_DealerBikeModel_13327_12",
"BW_DealerBikeModel_13329_10",
"BW_DealerBikeModel_13334_13",
"BW_DealerBikeModel_13343_12",
"BW_DealerBikeModel_13398_9",
"BW_DealerBikeModel_13883_1",
"BW_DealerBikeModel_13891_1",
"BW_DealerBikeModel_14102_6",
"BW_DealerBikeModel_14243_7",
"BW_DealerBikeModel_14368_13",
"BW_DealerBikeModel_14376_11",
"BW_DealerBikeModel_14387_10",
"BW_DealerBikeModel_14409_13",
"BW_DealerBikeModel_14418_9",
"BW_DealerBikeModel_14495_12",
"BW_DealerBikeModel_14516_12",
"BW_DealerBikeModel_14538_12",
"BW_DealerBikeModel_14559_1",
"BW_DealerBikeModel_14603_17",
"BW_DealerBikeModel_14638_15",
"BW_DealerBikeModel_14650_1",
"BW_DealerBikeModel_14713_7",
"BW_DealerBikeModel_14716_7",
"BW_DealerBikeModel_14736_1",
"BW_DealerBikeModel_14738_1",
"BW_DealerBikeModel_14752_1",
"BW_DealerBikeModel_14820_15",
"BW_DealerBikeModel_15005_13",
"BW_DealerBikeModel_15010_8",
"BW_DealerBikeModel_15265_6",
"BW_DealerBikeModel_15356_1",
"BW_DealerBikeModel_15357_1",
"BW_DealerBikeModel_15589_6",
"BW_DealerBikeModel_15800_7",
"BW_DealerBikeModel_10267_1",
"BW_DealerBikeModel_10241_6",
"BW_DealerBikeModel_10254_13",
"BW_DealerBikeModel_10675_10",
"BW_DealerBikeModel_10752_15",
"BW_DealerBikeModel_10865_7",
"BW_DealerBikeModel_11348_7",
"BW_DealerBikeModel_11349_7",
"BW_DealerBikeModel_10248_11",
"BW_DealerBikeModel_10246_7",
"BW_DealerBikeModel_12527_6",
"BW_DealerBikeModel_12629_11",
"BW_DealerBikeModel_12631_13",
"BW_DealerBikeModel_13125_12",
"BW_DealerBikeModel_13126_12",
"BW_DealerBikeModel_13344_12",
"BW_DealerBikeModel_14390_8",
"BW_DealerBikeModel_14434_15",
"BW_DealerBikeModel_14442_10",
"BW_DealerBikeModel_14458_1",
"BW_DealerBikeModel_14476_7",
"BW_DealerBikeModel_14480_7",
"BW_DealerBikeModel_14481_7",
"BW_DealerBikeModel_14484_7",
"BW_DealerBikeModel_14646_1",
"BW_DealerBikeModel_14649_1",
"BW_DealerBikeModel_14717_7",
"BW_DealerBikeModel_14740_1",
"BW_DealerBikeModel_14757_1",
"BW_DealerBikeModel_14761_9",
"BW_DealerBikeModel_14884_9",
"BW_DealerBikeModel_15011_6",
"BW_DealerBikeModel_15793_6",
"BW_DealerBikeModel_14763_17",
"BW_DealerBikeModel_16132_13",
"BW_DealerBikeModel_16202_7",
"BW_DealerBikeModel_16329_15",
"BW_DealerBikeModel_16330_15",
"BW_DealerBikeModel_16385_15",
"BW_DealerBikeModel_16455_12",
"BW_DealerBikeModel_16461_1",
"BW_DealerBikeModel_16466_9",
"BW_DealerBikeModel_16469_9",
"BW_DealerBikeModel_16543_15",
"BW_DealerBikeModel_16575_15",
"BW_DealerBikeModel_10552_7",
"BW_DealerBikeModel_11404_6",
"BW_DealerBikeModel_11983_7",
"BW_DealerBikeModel_12624_1",
"BW_DealerBikeModel_14369_13",
"BW_DealerBikeModel_14370_13",
"BW_DealerBikeModel_14540_12",
"BW_DealerBikeModel_14739_1",
"BW_DealerBikeModel_14754_1",
"BW_DealerBikeModel_14756_1",
"BW_DealerBikeModel_14883_1",
"BW_DealerBikeModel_15062_12",
"BW_DealerBikeModel_15801_7",
"BW_DealerBikeModel_15936_7",
"BW_DealerBikeModel_16462_1",
"BW_DealerBikeModel_16464_1",
"BW_DealerBikeModel_16468_1",
"BW_DealerBikeModel_21341_13",
"BW_DealerBikeModel_10247_66",
"BW_DealerBikeModel_10247_11",
"BW_DealerBikeModel_21382_13",
"BW_DealerBikeModel_10243_17",
"BW_DealerBikeModel_20467_15",
"BW_DealerBikeModel_10252_16",
"BW_DealerBikeModel_21354_7",
"BW_DealerBikeModel_21184_7",
"BW_DealerBikeModel_14762_17",
"BW_DealerBikeModel_20673_12",
"BW_DealerBikeModel_14602_17",
"BW_DealerBikeModel_10240_6",
"BW_DealerBikeModel_10242_7",
"BW_DealerBikeModel_10262_15",
"BW_DealerBikeModel_14761_17",
"BW_DealerBikeModel_14606_17",
"BW_DealerBikeModel_20522_7",
"BW_DealerBikeModel_10244_9",
"BW_DealerBikeModel_14759_17",
"BW_DealerBikeModel_14760_17",
"BW_DealerBikeModel_21290_7",
"BW_DealerBikeModel_21456_12",
"BW_DealerBikeModel_21468_13",
"BW_DealerBikeModel_21494_12",
"BW_DealerBikeModel_21542_6",
"BW_DealerBikeModel_21609_15",
"BW_DealerBikeModel_21645_15",
"BW_DealerBikeModel_21610_13",
"BW_DealerBikeModel_21641_13",
"BW_DealerBikeModel_21666_40",
"BW_DealerBikeModel_21702_40",
"BW_DealerBikeModel_14440_2",
"BW_DealerBikeModel_14443_9",
"BW_DealerBikeModel_14443_17",
"BW_DealerBikeModel_21729_13",
"BW_DealerBikeModel_21637_6",
"BW_DealerBikeModel_21621_13",
"BW_DealerBikeModel_21732_13",
"BW_DealerBikeModel_21731_13",
"BW_DealerBikeModel_21733_13",
"BW_DealerBikeModel_21748_11",
"BW_DealerBikeModel_21763_7",
"BW_DealerBikeModel_21764_7",
"BW_DealerBikeModel_21765_6",
"BW_DealerBikeModel_21766_7",
"BW_DealerBikeModel_21767_6",
"BW_DealerBikeModel_21804_11",
"BW_DealerBikeModel_21808_11",
"BW_DealerBikeModel_21809_7",
"BW_DealerBikeModel_21810_13",
"BW_DealerBikeModel_21811_11",
"BW_DealerBikeModel_21829_13",
"BW_DealerBikeModel_21846_11",
"BW_DealerBikeModel_21847_13",
"BW_DealerBikeModel_21848_13",
"BW_DealerBikeModel_21849_7",
"BW_DealerBikeModel_21850_1",
"BW_DealerBikeModel_21851_9",
"BW_DealerBikeModel_21851_17",
"BW_DealerBikeModel_21852_13",
"BW_DealerBikeModel_21853_13",
"BW_DealerBikeModel_21854_7",
"BW_DealerBikeModel_21861_6",
"BW_DealerBikeModel_21863_11",
"BW_DealerBikeModel_21864_11",
"BW_DealerBikeModel_21865_12",
"BW_DealerBikeModel_21869_7",
"BW_DealerBikeModel_21868_13",
"BW_DealerBikeModel_21870_13",
"BW_DealerBikeModel_21673_13",
"BW_DealerBikeModel_21348_2",
"BW_DealerBikeModel_21900_13",
"BW_DealerBikeModel_21623_1",
"BW_DealerBikeModel_21894_1",
"BW_DealerBikeModel_21895_1",
"BW_DealerBikeModel_21896_1",
"BW_DealerBikeModel_21897_9",
"BW_DealerBikeModel_21897_17",
"BW_DealerBikeModel_21898_17",
"BW_DealerBikeModel_21898_9",
"BW_DealerBikeModel_21614_40",
"BW_DealerBikeModel_21384_15",
"BW_DealerBikeModel_21913_7",
"BW_DealerBikeModel_21910_15",
"BW_DealerBikeModel_21911_16",
"BW_DealerBikeModel_21911_2",
"BW_DealerBikeModel_21380_11",
"BW_DealerBikeModel_21703_11",
"BW_DealerBikeModel_21704_7",
"BW_DealerBikeModel_21343_6",
"BW_DealerBikeModel_21914_6",
"BW_DealerBikeModel_21915_6",
"BW_DealerBikeModel_21919_6",
"BW_DealerBikeModel_21574_13",
"BW_DealerBikeModel_21924_12",
"BW_DealerBikeModel_21927_42",
"BW_DealerBikeModel_21943_9",
"BW_DealerBikeModel_21943_17",
"BW_DealerBikeModel_21665_7",
"BW_DealerBikeModel_21386_7",
"BW_DealerBikeModel_21922_7",
"BW_DealerBikeModel_21957_42",
"BW_DealerBikeModel_21932_12",
"BW_DealerBikeModel_21969_12",
"BW_DealerBikeModel_21974_12",
"BW_DealerBikeModel_21983_15",
"BW_DealerBikeModel_21663_11",
"BW_DealerBikeModel_21985_42",
"BW_DealerBikeModel_21986_13",
"BW_DealerBikeModel_22016_6",
"BW_DealerBikeModel_21695_6",
"BW_DealerBikeModel_22020_6",
"BW_DealerBikeModel_22025_1",
"BW_DealerBikeModel_22030_13",
"BW_DealerBikeModel_22031_6",
"BW_DealerBikeModel_22032_11",
"BW_DealerBikeModel_22039_13",
"BW_DealerBikeModel_22052_13",
"BW_DealerBikeModel_22053_13",
"BW_DealerBikeModel_22054_13",
"BW_DealerBikeModel_22078_7",
"BW_DealerBikeModel_22079_7",
"BW_DealerBikeModel_22080_7",
"BW_DealerBikeModel_22081_7",
"BW_DealerBikeModel_21558_13",
"BW_DealerBikeModel_21605_13",
"BW_DealerBikeModel_22150_12",
"BW_DealerBikeModel_22204_6",
"BW_DealerBikeModel_22305_6",
"BW_DealerBikeModel_22347_4",
"BW_DealerBikeModel_22470_12",
"BW_DealerBikeModel_22474_11",
"BW_DealerBikeModel_21643_6",
"BW_DealerBikeModel_22476_7",
"BW_DealerBikeModel_22477_5",
"BW_DealerBikeModel_22478_7",
"BW_DealerBikeModel_22485_6",
"BW_DealerBikeModel_22486_6",
"BW_DealerBikeModel_22488_1",
"BW_DealerBikeModel_22489_9",
"BW_DealerBikeModel_15978_17",
"BW_DealerBikeModel_22491_41",
"BW_DealerBikeModel_22493_1",
"BW_DealerBikeModel_22494_10",
"BW_DealerBikeModel_22495_1",
"BW_DealerBikeModel_22500_42",
"BW_DealerBikeModel_22501_42",
"BW_DealerBikeModel_22505_12",
"BW_DealerBikeModel_22506_6",
"BW_DealerBikeModel_22510_13",
"BW_DealerBikeModel_22511_42",
"BW_DealerBikeModel_22512_42",
"BW_DealerBikeModel_22513_9",
"BW_DealerBikeModel_22513_17",
"BW_DealerBikeModel_22520_13",
"BW_DealerBikeModel_22521_16",
"BW_DealerBikeModel_22522_16",
"BW_DealerBikeModel_22523_16",
"BW_DealerBikeModel_22524_7",
"BW_DealerBikeModel_21654_42",
"BW_DealerBikeModel_22526_11",
"BW_DealerBikeModel_22529_15",
"BW_DealerBikeModel_22530_15",
"BW_DealerBikeModel_22531_6",
"BW_DealerBikeModel_22533_20",
"BW_DealerBikeModel_22534_34",
"BW_DealerBikeModel_22535_5",
"BW_DealerBikeModel_22549_13",
"BW_DealerBikeModel_21445_7",
"BW_DealerBikeModel_22554_17",
"BW_DealerBikeModel_22555_17",
"BW_DealerBikeModel_22556_17",
"BW_DealerBikeModel_22557_15",
"BW_DealerBikeModel_13398_17",
"BW_DealerBikeModel_22559_7",
"BW_DealerBikeModel_22561_5",
"BW_DealerBikeModel_22562_5",
"BW_DealerBikeModel_22563_5",
"BW_DealerBikeModel_22575_11",
"BW_DealerBikeModel_22579_13",
"BW_DealerBikeModel_22593_1",
"BW_DealerBikeModel_22594_40",
"BW_DealerBikeModel_22599_9",
"BW_DealerBikeModel_22599_17",
"BW_DealerBikeModel_22600_15",
"BW_DealerBikeModel_22601_4",
"BW_DealerBikeModel_22603_7",
"BW_DealerBikeModel_22604_6",
"BW_DealerBikeModel_22605_6",
"BW_DealerBikeModel_22606_7",
"BW_DealerBikeModel_22607_2",
"BW_DealerBikeModel_22607_20",
"BW_DealerBikeModel_22610_7",
"BW_DealerBikeModel_22611_42",
"BW_DealerBikeModel_22612_42",
"BW_DealerBikeModel_22613_42",
"BW_DealerBikeModel_22615_42",
"BW_DealerBikeModel_22616_42",
"BW_DealerBikeModel_21383_11"};
            foreach (string key in keys)
            {
                _mc1.Remove(key);
				HttpContext.Current.Response.Write("Given key '" + key + "' - object removed from the memcache. On Page");
            }
                        /*var cacheObject1 = _mc1.Get(key1);
                
                        if (cacheObject1 != null)
                        {
				        HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(cacheObject1));
				        HttpContext.Current.Response.Write("<br />");
                        HttpContext.Current.Response.Write("Given key '" + key1 + "' - object exists in the memcache. On Page");
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("Given key '" + key1 + "' - object do not exists in the memcache. On Page");
                        }*/
			  
                        bool refreshKey = chkClearCache.Checked;
			  
                        if(refreshKey)
                        {
                          _mc1.Remove(key1);
                          HttpContext.Current.Response.Write("Given key '" + key1 + "' - object removed from the memcache. On Page");
                        }
						else{
							HttpContext.Current.Response.Write("Given key '" + key1 + "' - object not removed from the memcache. On Page");
						}*/						
	        }
			}catch(Exception ex){
			Bikewale.Notifications.ErrorClass err = new Bikewale.Notifications.ErrorClass(ex,"PageLoad");
			}
        }        
        
        </script>    
</body>
</html>
