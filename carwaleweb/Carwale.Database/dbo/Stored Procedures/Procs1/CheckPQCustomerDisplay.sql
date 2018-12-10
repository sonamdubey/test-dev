IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckPQCustomerDisplay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckPQCustomerDisplay]
GO

	-- =============================================
-- Author:		<vinayak>
-- Create date: <7/29/2014>
-- Description:	<Checks for the Make and City whether the customer contact fields are required for the PQ page>
-- =============================================
CREATE PROCEDURE [dbo].[CheckPQCustomerDisplay] --[CheckPQCustomerDisplay] 0,12
	-- Add the parameters for the stored procedure here
	@MakeId Int,
	@CityId Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT COUNT(1) FROM PQCustomerFormRules WITH (NOLOCK)
	WHERE	(
				(MakeId=@MakeId AND CityId=-99)
				OR
				(MakeId=-99 AND CityId=@CityId)
				OR
				(MakeId=@MakeId AND CityId=@CityId)

			)
			AND IsActive = 1
END
