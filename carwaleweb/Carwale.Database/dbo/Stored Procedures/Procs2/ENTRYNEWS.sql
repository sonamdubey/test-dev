IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ENTRYNEWS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ENTRYNEWS]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CAR NEWS

CREATE PROCEDURE [dbo].[ENTRYNEWS]
	@ID			NUMERIC,		--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@MAKEID		NUMERIC,		--ID OF THE MAKE
	@TITLE		VARCHAR(200),		--TITLE OF THE NEWS
	@Summary		VARCHAR(200),		--TITLE OF THE NEWS
	@Source		VARCHAR(50),		--Source of the news
	@SourceLink		VARCHAR(500),		--Link of the source
	@NEWSDATE 		DATETIME,		--DATE OF THE NEWS
	@IsPressRelease	BIT,			-- If the news is press release?
	@IsPublished		BIT,			-- Is it published?
	@NEWSID		NUMERIC OUTPUT
 AS
	
BEGIN
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO NEWS(MAKEID, TITLE, Summary, NEWSDATE, Source, SourceLink, ISACTIVE,IsPressRelease,IsPublished) 
			VALUES(@MAKEID, @TITLE, @Summary,  @NEWSDATE,@Source,@SourceLink, 1,@IsPressRelease,@IsPublished)

		SET @NEWSID = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE NEWS SET 
			MAKEID	= @MAKEID,
			TITLE 		= @TITLE,
			Summary	= @Summary,
			Source		= @Source,
			SourceLink	= @SourceLink,
			NEWSDATE 	= @NEWSDATE,
			IsPressRelease	= @IsPressRelease,
			IsPublished	= @IsPublished
		 WHERE 
			ID = @ID

		SET @NEWSID = @ID
	END
	
		
END