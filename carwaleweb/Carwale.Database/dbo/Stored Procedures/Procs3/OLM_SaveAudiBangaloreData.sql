IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveAudiBangaloreData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveAudiBangaloreData]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 17-Apr-2013
-- Description:	To save data fot Audi Bangalore dealer test drive lead(Table - OLM_AudiBangalore) and use it to mail it to dealer
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveAudiBangaloreData]
	-- Add the parameters for the stored procedure here
	@FullName			VARCHAR(100),
	@Mobile				VARCHAR(15),
	@City				VARCHAR(50),
	@Email				VARCHAR(100),
	@Profession			VARCHAR(100),
	@CurrentCar			VARCHAR(100),
	@NewId				NUMERIC(18,0) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    SET @NewId = -1
    
	INSERT INTO OLM_AudiBangalore ( FullName, Mobile, City, Email, Profession, CurrentCar )
	VALUES ( @FullName, @Mobile, @City, @Email, @Profession, @CurrentCar )
	
	SET @NewId = SCOPE_IDENTITY()
END
