IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsuranceCompanySave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsuranceCompanySave]
GO

	
-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 5th October 2011
-- Description:	This procedure is used to add update dealer's Bank
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsuranceCompanySave]
(
@TC_InsuranceCompany_Id INT =NULL,
@DealerId NUMERIC,
@CompanyName VARCHAR(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@TC_InsuranceCompany_Id IS NULL) --Insering Dealer's Insurance Company
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_InsuranceCompany WHERE DealerId=@DealerId AND CompanyName=@CompanyName AND IsActive=1)
		BEGIN
			INSERT TC_InsuranceCompany(CompanyName,DealerId) VALUES(@CompanyName,@DealerId)
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating Dealer's  Insurance Company
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_InsuranceCompany WHERE DealerId=@DealerId AND TC_InsuranceCompany_Id<>@TC_InsuranceCompany_Id AND CompanyName=@CompanyName AND IsActive=1)
		BEGIN
			UPDATE TC_InsuranceCompany SET CompanyName=@CompanyName WHERE TC_InsuranceCompany_Id=@TC_InsuranceCompany_Id
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END 	
	SELECT TC_InsuranceCompany_Id ,CompanyName FROM TC_InsuranceCompany WHERE DealerId=@DealerId AND IsActive=1    
END



