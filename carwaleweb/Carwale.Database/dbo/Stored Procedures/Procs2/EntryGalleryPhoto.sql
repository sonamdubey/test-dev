IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryGalleryPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryGalleryPhoto]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CAR WALLPAPERS
--ID, Title, ModelId, VersionId, VersionSpecific, Eight, Thousand, RandomString, EightFileSize, ThousandFileSize

CREATE PROCEDURE [dbo].[EntryGalleryPhoto]
	@ID			NUMERIC,		--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@TITLE		VARCHAR(50),		--TITLE 
	@MODELID		NUMERIC,		--ID OF THE MODEL
	@VERSIONID		NUMERIC,		--ID OF THE VERSION
	@PhotoName		VARCHAR(200),		--THE INITIALS OF THE FILE
	@CategoryId		INT,			-- Category Id
	@ENTRYDATE 	DATETIME		--DATE
	
 AS
	
BEGIN
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO CarGallery
			( TITLE, MODELID, VERSIONID, PhotoName, CategoryId, ENTRYDATE) 
		VALUES
			( @TITLE, @MODELID, @VERSIONID, @PhotoName, @CategoryId, @ENTRYDATE)
		
		
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		
		UPDATE CarGallery SET 
			TITLE = @TITLE, CategoryId=@CategoryId
		WHERE
			ID = @ID	
	END
	
		
END
