IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInqCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInqCustomerDetails]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 11 Jan 2012
-- Description:	This procedure is used to get Inquiry customer details 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInqCustomerDetails]
@StockId INT
AS
BEGIN

	Select ( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) AS CarName, RegNo,St.MakeYear ,St.Kms AS Kms,St.Price  ,Cc.RegistrationPlace FROM TC_Stock St 
				 INNER JOIN CarVersions Ve ON Ve.Id = St.VersionId 
				 INNER JOIN CarModels Mo ON Mo.Id = Ve.CarModelId
				 INNER JOIN CarMakes Ma ON Ma.Id = Mo.CarMakeId 
				 INNER JOIN TC_CarCondition Cc ON Cc.StockId=St.Id
				 WHERE St.Id = @StockId
				 
				 
	SELECT Id, BuyTime FROM TC_BuyTimePreference WHERE IsActive=1
	SELECT TOP 3 Id, Status FROM TC_InquiryStatus WHERE IsActive=1
	--SELECT ID, Name FROM CarMakes WHERE IsDeleted = 0 ORDER BY Name
END

