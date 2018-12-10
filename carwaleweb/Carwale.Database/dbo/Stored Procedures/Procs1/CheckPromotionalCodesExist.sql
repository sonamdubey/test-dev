IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckPromotionalCodesExist]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckPromotionalCodesExist]
GO

	-- =============================================  
-- Author:  Vikas  
-- Create date: 14-12-2012  
-- Description: To check how many offer codes are present in the table that are not past the current date  
-- =============================================  
CREATE PROCEDURE [dbo].[CheckPromotionalCodesExist]  
	@PackageId	INT 
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
 DECLARE @ValidOfferExists BIT  
  
 IF EXISTS(SELECT ID FROM PromotionalOffers WHERE GETDATE() <= OfferValidity AND ConsumerType = 2 AND PackageId=@PackageId)  
  SET @ValidOfferExists = 1  
 ELSE  
  SET @ValidOfferExists = 0  
    
 SELECT @ValidOfferExists AS ValidOfferExists   
END
