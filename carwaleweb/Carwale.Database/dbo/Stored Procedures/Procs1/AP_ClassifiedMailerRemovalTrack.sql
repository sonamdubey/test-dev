IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_ClassifiedMailerRemovalTrack]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_ClassifiedMailerRemovalTrack]
GO

	-- Author:		Khushaboo Patil 
-- Create date: 03-Feb-2015
-- Description:	Get Classified MailerTrack from CustomerSellInquiries and Insert into CH_UnpaidMailerTrack
-- Modified BY : Khushaboo Patil on 17/03/2015 Removed sms frequency for 90 days
				
CREATE PROCEDURE [dbo].[AP_ClassifiedMailerRemovalTrack]	
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
								Responses	 SMALLINT,	
								ViewCount	 NUMERIC(18,0), 
								CarRegNo	 VARCHAR(50),
								ExpDate		 DATETIME,
								ProfileId	 VARCHAR(50))
	
	INSERT INTO @TempMailer (InquiryId,CustomerId,MailerDay,CustomerName,email,Mobile,CarName,Responses,ViewCount,CarRegNo,ExpDate,ProfileId)	

	--Frequency of SMS (30,40,50,60,75)
	SELECT CSI.Id ,CSI.CustomerId,DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate),CSI.CustomerName AS CustName,CSI.CustomerEmail AS Email,
	CSI.CustomerMobile AS Mobile,(CMA.Name + ' ' + CMO.Name) AS CarName, LL.Responses,CSI.ViewCount,  CarRegNo,
	CSI.ClassifiedExpiryDate AS ExpDate,LL.ProfileId AS ProfileId
	FROM CustomerSellInquiries CSI WITH(NOLOCK),
	CarMakes CMA WITH(NOLOCK), CarModels CMO WITH(NOLOCK), CarVersions CV WITH(NOLOCK), livelistings AS LL WITH(NOLOCK)
	WHERE CSI.CarVersionId = CV.Id AND CV.CarModelId = CMO.Id AND CMO.CarMakeId = CMA.Id
	AND LL.Inquiryid = CSI.ID AND LL.SellerType = 2
	AND
	(
		DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate) = 30 OR DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate) = 50
		OR DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate) = 60 OR DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate) = 40
		OR DATEDIFF(DD, GETDATE(), CSI.ClassifiedExpiryDate) = 15
	)    	           
	ORDER BY ExpDate ASC
	
	INSERT INTO CH_UnpaidMailerTrack (InquiryId,CustomerId,MailDate,MailerDay,MailerTypesId)
	SELECT InquiryId,CustomerId,GETDATE(), MailerDay, 3 FROM @TempMailer
	
	SELECT TM.CustomerName AS CustName,TM.email,TM.Mobile,TM.CarName,TM.ViewCount , TM.Responses , TM.ProfileId , TM.CarRegNo 
	FROM @TempMailer TM
	
END

