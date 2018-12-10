IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMiscellaneousScripts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMiscellaneousScripts]
GO

	-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 21-6-2016
-- Description:	Hashtable for miscellaneous scripts
-- =============================================
create PROCEDURE [dbo].[GetMiscellaneousScripts]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SELECT HKEY,HVALUE from MiscellaneousScripts with(nolock)
END