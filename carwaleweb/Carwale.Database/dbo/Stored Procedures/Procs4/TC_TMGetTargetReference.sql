IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetTargetReference]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetTargetReference]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 16/12/2013
-- Description:	Get Reference for View target
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetTargetReference]
AS
BEGIN
	SELECT TC_TargetTypeId AS Value,TargetType AS Text FROM TC_TargetType WHERE IsActive=1
END
