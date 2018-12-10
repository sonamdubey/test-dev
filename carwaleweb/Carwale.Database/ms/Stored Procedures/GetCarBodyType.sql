IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[ms].[GetCarBodyType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [ms].[GetCarBodyType]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 25/04/2012
-- Description:	This Sp Returns body type of car
-- =============================================
CREATE PROCEDURE [ms].[GetCarBodyType]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select ID,Name from CarBodyStyles
END