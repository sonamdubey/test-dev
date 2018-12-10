IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveUploadedPricesFilesData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveUploadedPricesFilesData]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <11/7/2014>
-- Description:	<Insert Uploaded prices files data>
-- =============================================
CREATE PROCEDURE [dbo].[Con_SaveUploadedPricesFilesData]
	@SelectedCity [dbo].[Con_City] READONLY,
	@SelectedModel [dbo].[Con_Model] READONLY,
	@MakelId BIGINT,
	@StateId BIGINT,
	@UploadedBy	VARCHAR(50),
	@FileName	VARCHAR(100),
	@HostUrl VARCHAR(100)
AS
BEGIN	 
	INSERT INTO Con_UploadedPricesFiles 
	SELECT @MakelId,ModelId,@StateId,CityId,@UploadedBy,GETDATE(),@FileName,@HostUrl
	FROM @SelectedCity 
	CROSS JOIN @SelectedModel
END
