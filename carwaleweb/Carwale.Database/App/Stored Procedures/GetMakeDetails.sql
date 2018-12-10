IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetMakeDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetMakeDetails]
GO

	
-- ===================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for Make Details of particular make
-- ===================================================

CREATE PROCEDURE [App].[GetMakeDetails] 
	 @ID Integer
AS
BEGIN

	SELECT Name, LogoUrl
	FROM CarMakes
	WHERE ID = @ID	

END

