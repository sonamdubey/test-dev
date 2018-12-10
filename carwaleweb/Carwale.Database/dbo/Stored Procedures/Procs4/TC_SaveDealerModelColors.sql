IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveDealerModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveDealerModelColors]
GO

	
-------------------------------------------------------
-- =============================================
-- Author:		<Harsh Patel>
-- Create date: < 2 June 2015 >
-- Description:	<Save Dealer Model Colors>
-- Modified by:Komal manjare on 7 AUGUST 2015
--new parameter OriginalImgPath added 
--Modified By:Komal Manjare
--OriginalImgPath versioning 
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveDealerModelColors]
	@DealerId INT = NULL,
	@DWModelId INT = NULL,
	@ColorName VARCHAR(50) = NULL,
	@ColorCode VARCHAR(20) = NULL,
	@HostUrl VARCHAR(200) = NULL,
	@ImgPath VARCHAR(100) = NULL,
	@ImgName VARCHAR(50) = NULL,
	@OriginalImgPath VARCHAR(300)=NULL,
	@Type INT,
	@Id INT = NULL OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Todaydate DATETIME
	SET @Todaydate = GETDATE(); 

	IF(@ImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
	BEGIN
		SET @ImgName = @ImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@Todaydate,112)+ CONVERT(VARCHAR,@Todaydate,114),':','');
	END
	IF(@OriginalImgPath IS NOT NULL)
	BEGIN 
	SET @OriginalImgPath = @OriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@Todaydate,112)+ CONVERT(VARCHAR,@Todaydate,114),':','');
	END
	IF(@Type = 1) -- insert new record
	BEGIN
		INSERT INTO Microsite_DealerModelColors (DealerId,DWModelId,ColorName,ColorCode,HostUrl,ImgPath,ImgName,OriginalImgPath,IsActive,EntryDate)
		VALUES (@DealerId,@DWModelId,@ColorName,@ColorCode, @HostUrl,@ImgPath,@ImgName,@OriginalImgPath,1,GETDATE())

		SET @Id = SCOPE_IDENTITY();

	END

	ELSE IF(@Type = 2) -- UPDATE RECORD
	BEGIN
		UPDATE Microsite_DealerModelColors
		SET ColorName = ISNULL(@ColorName,ColorName), HostUrl = ISNULL(@HostUrl,HostUrl),
		ImgPath = ISNULL(@ImgPath,ImgPath),ImgName = ISNULL(@ImgName,ImgName),
		ColorCode = ISNULL(@ColorCode,ColorCode),OriginalImgPath=ISNULL(@OriginalImgPath,OriginalImgPath),
		ModifiedDate = GETDATE()
		WHERE Id = @Id
	END

	ELSE IF(@Type = 3) -- DEACTIVATE RECORD
	BEGIN
		UPDATE Microsite_DealerModelColors
		SET IsActive = 0
		WHERE Id = @Id
	END

	ELSE IF(@Type = 4) -- ACTIVATE RECORD
	BEGIN
		UPDATE Microsite_DealerModelColors
		SET IsActive = 1
		WHERE Id = @Id
	END

END