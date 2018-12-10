IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_UnpaidMailerTrackGetInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_UnpaidMailerTrackGetInsert]
GO

	-- Author:		Dilip V. 
-- Create date: 05-Sept-2012
-- Description:	Get Unpaid MailerTrack from CustomerSellInquiries and Insert into CH_UnpaidMailerTrack
				
CREATE PROCEDURE [dbo].[AP_UnpaidMailerTrackGetInsert]	
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
	FROM CustomerSellInquiries CS
	JOIN Customers C ON (CS.CustomerId=C.Id)
	JOIN CarVersions CV ON(CV.ID=CS.CarVersionId)
	JOIN CarModels CMO ON(CMO.ID=CV.CarModelId)
	JOIN CarMakes CMA ON (CMA.ID=CMO.CarMakeId)
	WHERE CONVERT(DATE,ClassifiedExpiryDate + 1) = CONVERT(DATE, GETDATE())

	--SELECT DISTINCT CS.ID InquiryId, C.Id CustId,DATEDIFF(DAY,EntryDate,GETDATE()) MailDay, C.Name AS CustomerName, C.email, C.Mobile, 
	--(CMA.Name + ' ' + CMO.Name) AS CarName, CMO.Id AS ModelId, CS.CarVersionId,
	--DAY(CS.EntryDate) AS ListDay, MONTH(CS.EntryDate) AS ListMonth, YEAR(CS.EntryDate) AS ListYear
	--FROM Customers AS C WITH(NOLOCK)
	--JOIN CustomerSellInquiries AS CS WITH(NOLOCK) ON CS.CustomerId = C.Id
	--JOIN CarVersions AS CV WITH(NOLOCK) ON CS.CarVersionId = CV.ID
	--JOIN CarModels AS CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID
	--JOIN CarMakes AS CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
	--WHERE (DAY(CS.EntryDate) =  DAY(GETDATE()-2)  OR DAY(CS.EntryDate) =  DAY(GETDATE()-5))
	--AND (MONTH(CS.EntryDate) =  MONTH(GETDATE()-2)  OR MONTH(CS.EntryDate) =  MONTH(GETDATE()-5))
	--AND (YEAR(CS.EntryDate) =  YEAR(GETDATE()-2)  OR YEAR(CS.EntryDate) =  YEAR(GETDATE()-5))
	--AND CS.PackageType <> 2 AND CS.IsFake <> 1 AND CS.StatusId NOT IN(2,3,4,6,7)
	--AND CS.SourceId <> 36 --AND CS.CityId NOT IN(SELECT Id FROM Cities WHERE StateId = 1)
	
	INSERT INTO CH_UnpaidMailerTrack (InquiryId,CustomerId,MailDate,MailerDay,MailerTypesId)
	SELECT InquiryId,CustomerId,GETDATE(), MailerDay, 1 FROM @TempMailer
	
	SELECT CUMT.Id MailerTrackId,TM.CustomerName,TM.email,TM.Mobile,TM.CarName,TM.ModelId,TM.ListDay,TM.ListMonth,TM.ListYear,TM.CarVersionId, TM.MailerDay 
	FROM @TempMailer TM
	JOIN CH_UnpaidMailerTrack CUMT ON TM.InquiryId = CUMT.InquiryId
	WHERE CONVERT(CHAR(10),CUMT.MailDate, 120) = CONVERT(CHAR(10),GETDATE(), 120)
	
END

