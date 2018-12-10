IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHDFCLeadCountForDay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHDFCLeadCountForDay]
GO

	-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 3-11-2014
-- Description:	HDFC Financial leads count for the current day
-- =============================================
CREATE PROCEDURE [dbo].[GetHDFCLeadCountForDay] -- GetHDFCLeadCountForDay NULL
	@Count int Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Count=0;

	SELECT @Count= COUNT(DISTINCT id) FROM NCS_TDREQ as t WITH(NOLOCK)  where t.HDFCResponse='success'
	and CONVERT(date,t.CreatedOn)=CONVERT(DATE,getdate());
	
	--SELECT DISTINCT * from NCS_TDReq
END

