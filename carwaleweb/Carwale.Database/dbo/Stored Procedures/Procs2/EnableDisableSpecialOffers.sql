IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EnableDisableSpecialOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EnableDisableSpecialOffers]
GO

	
-- =============================================      
-- Author:  <Piyush Sahu>
-- Create date: <9/29/2016>      
-- Description: <update isActive flag> 
-- =============================================     

CREATE PROCEDURE [dbo].[EnableDisableSpecialOffers]
@CompCategoryName varchar(100),
@MinLoanAmount int,
@MaxLoanAmount int,
@IsActive BIT

AS	
BEGIN
	DECLARE @CompCategoryId  int

	SELECT @CompCategoryId = Id FROM CW_CompanyCategories WITH(NOLOCK) WHERE CategoryName = @CompCategoryName

			
	UPDATE CW_FinanceSpecialOffers
	SET 
		IsCampaignActive = @IsActive
				
	WHERE CW_CompanyCategoryId = @CompCategoryId AND MinLoanAmount = @MinLoanAmount AND MaxLoanAmount = @MaxLoanAmount			
			
END

