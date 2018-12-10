IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveMansoonCampAnswer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveMansoonCampAnswer]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 7 July 2013
-- Description:	Save user's name, email, answer(place name) for SKODA mansoon campaign 2013
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveMansoonCampAnswer]
	-- Add the parameters for the stored procedure here
	@FullName		VARCHAR(50),
	@Email			VARCHAR(50),
	@Answer			VARCHAR(100),
	@PlaceId		SMALLINT,
	@NewId			NUMERIC(18,0) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SET @NewId = -1
    
	INSERT INTO OLM_MansoonCamp ( FullName, Email, Answer, PlaceId)
	VALUES ( @FullName, @Email, @Answer, @PlaceId)
	
	SET @NewId = SCOPE_IDENTITY()
END
