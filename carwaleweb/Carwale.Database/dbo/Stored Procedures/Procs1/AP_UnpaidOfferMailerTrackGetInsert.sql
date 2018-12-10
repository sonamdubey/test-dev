IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_UnpaidOfferMailerTrackGetInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_UnpaidOfferMailerTrackGetInsert]
GO

	
-- Author:		Dilip V. 
-- Create date: 05-Sept-2012
-- Description:	Get Unpaid MailerTrack from CustomerSellInquiries and Insert into CH_UnpaidMailerTrack
				
CREATE PROCEDURE [dbo].[AP_UnpaidOfferMailerTrackGetInsert]	
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @TempMailer Table(	InquiryId	 NUMERIC(18,0), 
								CustomerId	 NUMERIC(18,0),
								MailerDay	 INT,
								CustomerName VARCHAR(100),
								email		 VARCHAR(100), 
								Mobile		 VARCHAR(20),
								CarName		 VARCHAR(62),
								ModelId		 NUMERIC(18,0),
								ListDay		 TINYINT,
								ListMonth	 TINYINT,
								ListYear	 SMALLINT,
								CarVersionId NUMERIC(18,0))
	
	
	INSERT INTO @TempMailer (InquiryId,CustomerId,MailerDay,CustomerName,email,Mobile,CarName,ModelId,CarVersionId,ListDay,ListMonth,ListYear)	
	SELECT DISTINCT CS.ID InquiryId, C.Id CustId,DATEDIFF(DAY,CS.EntryDate,GETDATE()) MailDay, C.Name AS CustomerName, C.email, C.Mobile, 
		(CMA.Name + ' ' + CMO.Name) AS CarName, CMO.Id AS ModelId, CS.CarVersionId,
		DAY(CS.EntryDate) AS ListDay, MONTH(CS.EntryDate) AS ListMonth, YEAR(CS.EntryDate) AS ListYear
		FROM  --LiveListings LL JOIN
			 CustomerSellInquiries CS --ON (LL.Inquiryid=CS.Id)
			JOIN Customers C ON (CS.CustomerId=C.Id)
			JOIN CarVersions CV ON(CV.ID=CS.CarVersionId)
			JOIN CarModels CMO ON(CMO.ID=CV.CarModelId)
			JOIN CarMakes CMA ON (CMA.ID=CMO.CarMakeId)
			WHERE-- LL.SellerType = 2 AND
			 --CONVERT(VARCHAR(8),ClassifiedExpiryDate,112)='20140115'
			 ClassifiedExpiryDate BETWEEN '2014/01/23' AND '2014/01/29 11:59:59 PM' 
		

	/*SELECT DISTINCT CS.ID InquiryId, C.Id CustId,DATEDIFF(DAY,EntryDate,GETDATE()) MailDay, C.Name AS CustomerName, C.email, C.Mobile, 
	(CMA.Name + ' ' + CMO.Name) AS CarName, CMO.Id AS ModelId, CS.CarVersionId,
	DAY(CS.EntryDate) AS ListDay, MONTH(CS.EntryDate) AS ListMonth, YEAR(CS.EntryDate) AS ListYear
	FROM Customers AS C WITH(NOLOCK)
	JOIN CustomerSellInquiries AS CS WITH(NOLOCK) ON CS.CustomerId = C.Id
	JOIN CarVersions AS CV WITH(NOLOCK) ON CS.CarVersionId = CV.ID
	JOIN CarModels AS CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID
	JOIN CarMakes AS CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
	--JOIN CH_Calls AS CC WITH(NOLOCK) ON CS.Id = CC.EventId
	--JOIN CH_Logs AS CL WITH(NOLOCK) ON CC.Id = CL.CallId
	WHERE --CC.CallType = 1 AND CC.TBCType = 2 AND CL.ActionId NOT IN(48,45,46) AND 
	CS.EntryDate BETWEEN '2013/09/06' AND GETDATE()               --'2013/05/14 11:59:59 PM'
	AND ISNULL(CS.PackageType, 0) <> 2 AND CS.IsFake <> 1 AND CS.StatusId NOT IN(2,3,4,5,6,7,8)
	AND CS.PackageType IS NOT NULL AND ISNULL(CS.SourceId,0) <> 36 -- 36 for NCS-Cross Selling
	AND CS.CityId IN(SELECT Id FROM Cities WHERE StateId = 1)*/
	
	
	INSERT INTO CH_UnpaidMailerTrack (InquiryId,CustomerId,MailDate,MailerDay, MailerTypesId)
	SELECT InquiryId,CustomerId,GETDATE() + 1, MailerDay, 2 FROM @TempMailer
	
	SELECT CUMT.Id MailerTrackId,TM.CustomerName,TM.email,TM.Mobile,TM.CarName,TM.ModelId,TM.ListDay,TM.ListMonth,TM.ListYear,TM.CarVersionId 
	FROM @TempMailer TM
	JOIN CH_UnpaidMailerTrack CUMT ON TM.InquiryId = CUMT.InquiryId
	WHERE CONVERT(CHAR(10),CUMT.MailDate, 120) = CONVERT(CHAR(10),GETDATE()+1, 120)
	
END
