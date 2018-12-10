IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveDMSScreenShot]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveDMSScreenShot]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <28/07/2015>
-- Description:	<Save DMS ScreenShot>
-- Modified By Vivek Gupta on 12-08-2015, updated originalImgPath
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveDMSScreenShot]
	@DMSScreenShotHostUrl	VARCHAR(100),
	@DMSScreenShotUrl		VARCHAR(250),
	@TC_NewCarInquiriesId	INT
AS
BEGIN
	UPDATE TC_NewCarInquiries SET DMSScreenShotHostUrl = @DMSScreenShotHostUrl,DMSScreenShotUrl = @DMSScreenShotUrl , DMSScreenShotStatusId = 1,
	OriginalImgPath = @DMSScreenShotUrl
	WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
END












/****** Object:  StoredProcedure [dbo].[TC_INQSellerSave]    Script Date: 8/14/2015 11:50:29 AM ******/
SET ANSI_NULLS ON
