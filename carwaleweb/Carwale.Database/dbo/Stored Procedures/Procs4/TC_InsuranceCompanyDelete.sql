IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsuranceCompanyDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsuranceCompanyDelete]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 30th Nov 2011
-- Description:	Added status parameter
-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 5th October 2011
-- Description:	This procedure will be used to View All Dealers Bank for Finance
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsuranceCompanyDelete]
(
@DealerId NUMERIC,
@TC_InsuranceCompany_Id INT,
@Status INT OUTPUT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF NOT EXISTS(SELECT Top 1 * FROM TC_BookingInsurance WHERE TC_InsuranceCompany_Id=@TC_InsuranceCompany_Id AND IsActive=1)
		BEGIN
			UPDATE TC_InsuranceCompany  SET IsActive=0
			WHERE DealerId=@DealerId AND TC_InsuranceCompany_Id=@TC_InsuranceCompany_Id
			SET @Status=1
		END
		ELSE
		BEGIN
			SET @Status=2--record refrence with TC_BookingInsurance Table. so cant delete
		END
END

