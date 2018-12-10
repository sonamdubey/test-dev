IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryDetails]
GO

	-- Author:  Tejashree Patil  
-- Modified date: 11 April 2012  
-- Description: Getting interested in column with proper comma seperated.  
-- =============================================  
--Author: Binumon George  
--Date:30-03-2012  
--Descripton: geting all type of inquiry Details  
-- execute [TC_InquiryDetails] 1252,5,1  
-- Modified By : Tejashree Patil on 25 July 2012 Added select statement to get loose inq id(buyerInqWithoutStockId)and added src in @ViewMatch  
-- Modified by : Tejashree Patil on 21 Aug 2012 Check for null values in case of seller details in ProfileId and InsuranceExpiry  
-- Modified by : Tejashree Patil on 12 Oct 2012 Added SELECT statements where inquiryType is 5 onwords  
-- Modified By : Tejashree Patil on 15 Oct 2012 at 3 pm Declared new variable @SellCarInquiryId to get Seller Inquiry Id used to upload photo,and 
-- added IsPurchased in SELECT clause where InqType=2
-- Modified By : Tejashree Patil on 20 Nov 2012 at 3 pm Fetched column statusId of stock for buyer inquiry
-- =============================================  
CREATE PROCEDURE [dbo].[TC_InquiryDetails]  
@TC_InquiryId BIGINT,  
@BranchId INT,  
@InqType TINYINT  
AS  
BEGIN  
	IF(@InqType=1)--buyer  
		BEGIN    
            
		   SELECT DISTINCT INQT.InquiryType 'Inquiry Type ',  
			   --INQ.CreatedDate 'Inquiry Date',	  
			   COALESCE(ISNULL(INQ.CarName,'')+ISNULL(ModelNames,' ')+  
			   (CASE ISNULL(SUBSTRING(CONVERT(VARCHAR(12),ST.MakeYear,106),3,9),'')WHEN '' THEN '' ELSE ''+', ' +   
			   (SUBSTRING(CONVERT(VARCHAR(12),ST.MakeYear,106),3,9)) END) +' '+   
			   (CASE ISNULL(ST.Colour,'')WHEN '' THEN '' ELSE ''+', ' + CONVERT(VARCHAR(10),ST.Colour) END) +' '+  
			   (CASE ISNULL(CONVERT(VARCHAR(10),ST.Price),'') WHEN '' THEN '' ELSE ' '+',Rs. ' + CONVERT(VARCHAR(10),ST.Price) END) +' ' +   
			   (CASE ISNULL(CONVERT(VARCHAR(10),ST.Kms),'') WHEN '' THEN '' ELSE ' '+', ' + CONVERT(VARCHAR(10),ST.Kms)+' kms, ' END) +  
			   CC.RegistrationPlace,  
			   ISNULL(INQ.CarName+', ','')+ISNULL(ModelNames+', ',' ')+  
			   (CASE WHEN (MinPrice IS NOT NULL AND MaxPrice  IS NOT NULL)
				   THEN   
				   CASE   
					WHEN MinPrice=0 THEN ' Below Rs. '+CONVERT(VARCHAR(10),MaxPrice)    
					ELSE 'Rs. '+CONVERT(VARCHAR(10),MinPrice)+' - '+CONVERT(VARCHAR(10),MaxPrice)+','   
				   END  
				   ELSE (CASE  WHEN (COALESCE(' Rs. '+CONVERT(VARCHAR(10),MinPrice),'Rs. '+CONVERT(VARCHAR(10),MaxPrice))IS NOT NULL)   
					  THEN COALESCE(' Rs. '+CONVERT(VARCHAR(10),MinPrice),'Rs. '+CONVERT(VARCHAR(10),MaxPrice))  
					  ELSE ''   
					  END)  
				END)+  
			   (CASE WHEN (FromMakeYear IS NOT NULL AND ToMakeYear  IS NOT NULL)   
				   THEN ' Year  '+CONVERT(VARCHAR(10),FromMakeYear)+' - '+CONVERT(VARCHAR(10),ToMakeYear)      
				   ELSE (CASE   
					  WHEN (COALESCE(' Year '+CONVERT(VARCHAR(10),FromMakeYear),', Year '+CONVERT(VARCHAR(10),ToMakeYear))IS NOT NULL)   
					  THEN COALESCE(' Year '+CONVERT(VARCHAR(10),FromMakeYear),', Year '+CONVERT(VARCHAR(10),ToMakeYear))+', '   
					  ELSE '' END)  
			   END)+  
			   (CASE WHEN (BodyType IS NOT NULL AND BodyTypeName IS NOT NULL) THEN ' '+BodyTypeName   
				  ELSE (CASE ISNULL(FuelType,'') WHEN '' THEN '' ELSE ISNULL(''+FuelTypeName,'') END)  END)  
			   +' '+' ')AS 'Interested In', SRC.Source AS 'Inquiry Source',  
			   (CASE ISNULL(CAST(ST.Id AS VARCHAR(10)) ,'') WHEN '' THEN 'N/A' ELSE CAST(BI.StockId AS VARCHAR(10)) END) AS 'Stock Id' ,
			   CONVERT(VARCHAR(10),ST.StatusId) AS 'StockStatus' , -- Modified By : Tejashree Patil on 20 Nov 2012
			   ST.IsActive AS 'IsActive'		  
		   FROM	   TC_Inquiries INQ WITH(NOLOCK)  
				   LEFT JOIN TC_BuyerInquiries BI WITH(NOLOCK)ON BI.TC_InquiriesId=INQ.TC_InquiriesId   
				   LEFT JOIN TC_BuyerInqWithoutStock BIW WITH(NOLOCK)ON BIW.TC_InquiriesId=INQ.TC_InquiriesId  
				   LEFT JOIN TC_Stock ST WITH(NOLOCK) ON ST.Id=BI.StockId   
				   LEFT JOIN TC_CarCondition CC WITH(NOLOCK) ON ST.Id=CC.StockId   
				   INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id  
				   INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId  
		   WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=1  
	END  
	 ELSE 
	 IF(@InqType=2)-- seller  
		  BEGIN  
			   SELECT DISTINCT INQT.InquiryType 'Inquiry Type ',  
					   --INQ.CreatedDate 'Inquiry Date', 
					   INQ.CarName + ',' +SUBSTRING(CONVERT(VARCHAR,SI.MakeYear, 106),3,9)  +' '+   
					   (CASE ISNULL(SI.Price,'')WHEN '' THEN '' ELSE ','+'Rs. '+CONVERT(VARCHAR(10),SI.Price) END) +   
					   (CASE ISNULL(SI.Kms,'') WHEN '' THEN '' ELSE ','+CONVERT(VARCHAR(10),SI.Kms)+' Kms ' END)+   
					   ISNULL(CONVERT(VARCHAR(10),SI.RegistrationPlace),'') AS 'Interested In ',  
					   SRC.Source 'Inquiry Source',   
					   (CASE ISNULL(SI.CWInquiryId,'') WHEN '' THEN 'N/A' ELSE + '<a href=//www.carwale.com/used/cardetails.aspx?car=S'+ CONVERT(VARCHAR(10),SI.CWInquiryId) + '>' + 'S'+CONVERT(VARCHAR(10),SI.CWInquiryId) +'</a> ' END)As ProfileId,  
					   --SI.CWInquiryId 'Profile Id',  
					   dbo.TC_GetOwnerValue(SI.Owners) AS 'No.of Owners',   
					   SI.Insurance,ISNULL(CONVERT( VARCHAR(10),SI.InsuranceExpiry),'N/A') AS 'InsuranceExpiry', SI.Tax AS 'One Time Tax', SI.CarDriven AS 'Driven In',   
					   dbo.TC_GetBoolValue(SI.Accidental) AS 'Is Car Accidental',   
					   dbo.TC_GetBoolValue(SI.FloodAffected) 'Is Flood Affected' ,
					   dbo.TC_GetBoolValue(SI.IsPurchased) 'Is Purchased'
					   -- Modified By : Tejashree Patil on 15 Oct 2012 at 3
					    
			   FROM	   TC_Inquiries INQ WITH(NOLOCK)   
					   LEFT JOIN TC_SellerInquiries SI WITH(NOLOCK)ON SI.TC_InquiriesId=INQ.TC_InquiriesId   
					   INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id  
					   INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId 
					    
			   WHERE   INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=2  
		  END  
	ELSE 
	IF(@InqType=3)--testDrive  
		  BEGIN  
			   SELECT	DISTINCT INQT.InquiryType 'Inquiry Type',  
					    INQ.CarName  AS 'Interested In ',  
					    dbo.GetFuelType(TD.FuelType) AS 'FuelType' ,
					    dbo.GetTransmissionType(TD.Transmission) AS 'Transmission Type', SRC.Source 'Inquiry Source'  
			   FROM		TC_Inquiries INQ WITH(NOLOCK)  
					    LEFT JOIN TC_TestdriveRequests TD WITH(NOLOCK)ON TD.TC_InquiriesId=INQ.TC_InquiriesId   
					    INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id  
					    INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId  
			   WHERE	INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=3  
		  END  
	ELSE 
	IF(@InqType=4)--Price Quotes  
		  BEGIN  
			   SELECT	DISTINCT INQT.InquiryType 'Inquiry Type',  
					    INQ.CarName  AS 'Interested In ',  
					    PQ.BuyTime, SRC.Source 'Inquiry Source'  
			   FROM		TC_Inquiries INQ WITH(NOLOCK)  
						LEFT JOIN TC_PriceQuoteRequests PQ WITH(NOLOCK)ON PQ.TC_InquiriesId=INQ.TC_InquiriesId   
						INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id  
						INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId  
			   WHERE	INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=4  
		  END  
	ELSE 
	IF(@InqType=5)--Service Rquest  
		  BEGIN  
			   SELECT	DISTINCT INQT.InquiryType 'Inquiry Type',  
					    INQ.CarName  AS 'Interested In ',  
					    dbo.GetServiceType(SR.TypeOfService)AS TypeOfService, SR.RegNo,  
					    CONVERT(varchar, SR.PreferredDate, 106) AS PreferredDate, SRC.Source 'Inquiry Source'  
			   FROM		TC_Inquiries INQ   
					    LEFT JOIN TC_ServiceRequests SR WITH(NOLOCK)ON SR.TC_InquiriesId=INQ.TC_InquiriesId   
					    INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id  
					    INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId  
			   WHERE	INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=5  
		  END  
	-- Modified by : Tejashree Patil on 12 Oct 2012 Added SELECT statements where inquiryType is 5 onwords 
	ELSE IF(@InqType=6)--CallInquiry  
		  BEGIN  
				SELECT	'Call Inquiry' AS 'Inquiry Type'  
				FROM	TC_Inquiries WITH(NOLOCK)
				WHERE	TC_LeadTypeId=2 AND BranchId=@BranchId AND InquiryType=6
		  END  
	ELSE IF(@InqType=7)--EmailInquiry  
		  BEGIN  
				SELECT 'Email Inquiry' AS 'Inquiry Type'  
				FROM	TC_Inquiries WITH(NOLOCK)
				WHERE	TC_LeadTypeId=2 AND BranchId=@BranchId AND InquiryType=7
		  END  
	ELSE IF(@InqType=8)--LoanInquiry  
		  BEGIN  
				SELECT 'Loan Inquiry' AS 'Inquiry Type'  
				FROM	TC_Inquiries WITH(NOLOCK)
				WHERE	TC_LeadTypeId=2 AND BranchId=@BranchId AND InquiryType=8 
		  END  
	ELSE IF(@InqType=9)--Insurance Inquiry  
		  BEGIN  
				SELECT 'Insurance Inquiry' AS 'Inquiry Type'  
				FROM	TC_Inquiries WITH(NOLOCK)
				WHERE	TC_LeadTypeId=2 AND BranchId=@BranchId AND InquiryType=9 
		  END  
	ELSE IF(@InqType=10)--LoanInquiry  
		  BEGIN  
				SELECT 'Grievance Redressal' AS 'Inquiry Type'  
				FROM	TC_Inquiries WITH(NOLOCK)
				WHERE	TC_LeadTypeId=2 AND BranchId=@BranchId AND InquiryType=10  
		  END  
		  -- retrieving here stockid or TC_SellerInquiriesId for viewmatch  
		  -- Modified By : Tejashree Patil on 25 July 2012 Added select statement to get loose inq id(buyerInqWithoutStockId)and added src in @ViewMatch  
		  -- src=2 if viewMatch from any stock or buyer inq    
	DECLARE @ViewMatch VARCHAR(50) 
	DECLARE @SellCarInquiryId BIGINT --Modified By: Tejashree Patil on 15 Oct 2012 at 3 pm
	  
	IF(@InqType=1)  
	   IF EXISTS (SELECT TOP 1 * FROM TC_BuyerInqWithoutStock B WHERE B.TC_InquiriesId= @TC_InquiryId )  
		BEGIN  
			 SELECT @ViewMatch='src=2&tcbuyInqId=' + CAST(BI.TC_BuyerInqWithoutStockId AS VARCHAR(10)) FROM TC_BuyerInqWithoutStock BI   
			 INNER JOIN TC_Inquiries TI ON BI.TC_InquiriesId=TI.TC_InquiriesId  
			 WHERE TI.TC_InquiriesId=@TC_InquiryId  
		END  
	ELSE   
		BEGIN  
			 SELECT @ViewMatch='src=2&stockId=' + CAST(BI.StockId AS VARCHAR(10)) FROM TC_BuyerInquiries BI   
			 INNER JOIN TC_Inquiries TI ON BI.TC_InquiriesId=TI.TC_InquiriesId  
			 WHERE TI.TC_InquiriesId=@TC_InquiryId  
		END  
	ELSE IF(@InqType=2)  
	   BEGIN      
			--Modified By: Tejashree Patil on 15 Oct 2012 at 3 pm
			SELECT @SellCarInquiryId = SI.TC_SellerInquiriesId -- src=3 if viewMatch from seller inq  
			FROM TC_SellerInquiries SI   
			INNER JOIN TC_Inquiries TI ON SI.TC_InquiriesId=TI.TC_InquiriesId  
			WHERE TI.TC_InquiriesId=@TC_InquiryId  		
							
			SET @ViewMatch='src=3&sellInqId='+CAST(@SellCarInquiryId AS VARCHAR(10))
				
	   END  
   ELSE  
	   BEGIN  
			SET @ViewMatch=''  
	   END  
	     
	  SELECT @ViewMatch AS QueryStringVal  , @SellCarInquiryId AS SellerInquiryId
END  
  
  
  
  
