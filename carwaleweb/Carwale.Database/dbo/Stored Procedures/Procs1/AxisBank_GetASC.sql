IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetASC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetASC]
GO

	

-- =============================================
-- Author:		Akansha
-- Create date: 14.01.2014
-- Description:	Get All ASC for DropDown
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetASC]
	
AS
BEGIN
	SELECT * FROM AxisBank_ASC ORDER BY Name
END

