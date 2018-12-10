IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarColourResearch_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarColourResearch_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 10 Feb, 2009
-- Description:	SP to record data of car color researched by the customer
-- =============================================
CREATE PROCEDURE [dbo].[CarColourResearch_SP]
	-- Add the parameters for the stored procedure here
	@ModelId		INT,
	@HexCode		VARCHAR(6),
	@IsFavourate	Bit,
	@EntryDate		DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CarColourResearch(ModelId, HexCode, IsFavourate, EntryDate)
	VALUES(@ModelId, @HexCode, @IsFavourate, @EntryDate)
END

