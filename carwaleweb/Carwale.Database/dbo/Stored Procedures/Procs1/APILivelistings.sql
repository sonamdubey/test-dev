IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[APILivelistings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[APILivelistings]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-02-2015
-- Description:	Call API for Livelistings operations
-- EXEC APILivelistings 'D630799', 'GET'
-- Modified By : Sadhana Upadhyay on 26 Feb 2015
-- Summary : Added ActionName paremeter to Sp
-- Modified by Shikhar on 18-06-2015 commneted line for receiving response from API
-- =============================================
CREATE PROCEDURE [dbo].[APILivelistings]
	@ProfileId VARCHAR(10),
	@Method CHAR(7),
	@ActionName AS VARCHAR(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- this Stored Procedure is called from trigger
	--DECLARE @HostUrls AS VARCHAR(100) = '172.16.1.72:9001,172.16.0.11:9005';  Local
	--DECLARE @HostUrls AS VARCHAR(100) = '192.168.1.13:9006'; Staging
	DECLARE @HostUrls AS VARCHAR(100) ='192.168.1.13:9007,192.168.1.14:9007' -- Production
	DECLARE @HostUrl AS VARCHAR(50)

	SET  @HostUrl = (SELECT TOP 1   ListMember from fnSplitCSVToChar(@HostUrls) ORDER BY NEWID())

    DECLARE @Object AS INT;
	--DECLARE @ResponseText AS BIT;
	DECLARE @ProfilePage  VARCHAR(200) ='http://'+ @HostUrl +'/api/classifiedes/?profileid='+@ProfileId+'&actionname='+ @ActionName
	--select @ProfilePage
	EXEC sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;

	EXEC sp_OAMethod @Object, 'open', NULL, @Method, @ProfilePage, 'false'

	EXEC sp_OAMethod @Object, 'send'

	-- Commented by Shikhar on 18-06-2015 commneted line for receiving response from API
	--EXEC sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
	--select @ResponseText
	EXEC sp_OADestroy @Object

END
