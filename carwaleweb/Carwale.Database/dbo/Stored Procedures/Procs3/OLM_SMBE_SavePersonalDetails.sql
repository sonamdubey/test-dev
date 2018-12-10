IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SMBE_SavePersonalDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SMBE_SavePersonalDetails]
GO

	-- =============================================
-- Author:	RAHUL KUMAR
-- Create date: 17-Dec-2013
-- Description:	Saves user details 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SMBE_SavePersonalDetails]
	-- Add the parameters for the stored procedure here
	@Id NUMERIC OUTPUT
	,@CustomerName VARCHAR(50)
	,@Mobile VARCHAR(15)
	,@Email VARCHAR(50)
	,@CityId INT
	,@ModelId INT
	,@IpAddress VARCHAR(20)
AS
BEGIN
	INSERT INTO OLM_BookingData (
		CustomerName
		,Mobile
		,Email
		,CityId
		,ModelId
		,IpAddress
		)
	VALUES (
		@CustomerName
		,@Mobile
		,@Email
		,@CityId
		,@ModelId
		,@IpAddress
		)

	SET @Id = SCOPE_IDENTITY()
END
