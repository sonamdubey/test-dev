IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Basic_Save_v]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Basic_Save_v]
GO

	
--Modified By:Prashant Vishe On 15 Nov 2013
--Modification:added parameter IsFeatured
-- Modified By : Ashwini Todkar - Added applicationId field.
--Modified BY:Rakesh Yadav, added IsNotified while inserting data
--Modified By Ajay Singh on 17 feb 2016 added PushNotification
--Modified By Jitendra singh on 13 may 2016 update complete url in the basic table
CREATE PROCEDURE [dbo].[Con_EditCms_Basic_Save_v.16.2.7] @CategoryId NUMERIC(18, 0)
	,@Title VARCHAR(250)
	,@DisplayDate DATETIME
	,@AuthorName VARCHAR(100)
	,@AuthorId NUMERIC(18, 0)
	,@Description VARCHAR(8000)
	,@LastUpdatedBy NUMERIC(18, 0)
	,@LastUpdatedTime DATETIME
	,@Url VARCHAR(200)
	,@EnteredBy NUMERIC(18, 0)
	,@EntryDate DATETIME
	,@SubCatId VARCHAR(2000)
	,@CFId VARCHAR(200)
	,@ValType VARCHAR(200)
	,@Value VARCHAR(2000)
	,@ID NUMERIC(18, 0) out
	,@IsSticky BIT
	,@StickyFromDate DATETIME
	,@StickyToDate DATETIME
	,@IsFeatured BIT
	,@SocialMediaLine VARCHAR(120) = NULL
	,@IsCompatibleNews BIT
	,@ApplicationId tinyint
	,@PhotoCredit varchar(250) = null
	,@IsNotified bit = 0
AS
BEGIN
	IF @IsSticky = 1
	BEGIN
		INSERT INTO Con_EditCms_Basic (
			CategoryId
			,Title
			,Url
			,DisplayDate
			,AuthorName
			,authorid
			,Description
			,LastUpdatedTime
			,LastUpdatedBy
			,EnteredBy
			,EntryDate
			,IsActive
			,IsSticky
			,StickyFromDate
			,StickyToDate
			,IsFeatured
			,IsCompatibleForNewsLetter
			,SocialMediaLine
			,ApplicationID
			,PhotoCredit
			,PushNotification
			,IsNotified
			)
		VALUES (
			@CategoryId
			,@Title
			,@Url
			,@DisplayDate
			,@AuthorName
			,@AuthorId
			,@Description
			,@LastUpdatedTime
			,@LastUpdatedBy
			,@EnteredBy
			,@EntryDate
			,1
			,1
			,@StickyFromDate
			,@StickyToDate
			,@IsFeatured
			,@IsCompatibleNews
			,@SocialMediaLine
			,@ApplicationId
			,@PhotoCredit
			,@IsNotified
			,0
			)
	END
	ELSE
	BEGIN
		INSERT INTO Con_EditCms_Basic (
			CategoryId
			,Title
			,Url
			,DisplayDate
			,AuthorName
			,authorid
			,Description
			,LastUpdatedTime
			,LastUpdatedBy
			,EnteredBy
			,EntryDate
			,IsActive
			,IsSticky
			,StickyFromDate
			,StickyToDate
			,IsFeatured
			,IsCompatibleForNewsLetter
			,SocialMediaLine
			,ApplicationID
			,PhotoCredit
			,PushNotification
			,IsNotified
			)
		VALUES (
			@CategoryId
			,@Title
			,@Url
			,@DisplayDate
			,@AuthorName
			,@AuthorId
			,@Description
			,@LastUpdatedTime
			,@LastUpdatedBy
			,@EnteredBy
			,@EntryDate
			,1
			,NULL
			,NULL
			,NULL
			,@IsFeatured
			,@IsCompatibleNews
			,@SocialMediaLine
			,@ApplicationId
			,@PhotoCredit
			,@IsNotified
			,0
			)
	END

	SET @ID = SCOPE_IDENTITY()

	-- Jitendra singh on 13 may 2016 update complete url in the basic table
	IF @ApplicationId = 1 AND @ID > 0 AND @CategoryId NOT IN (8,13)
	BEGIN
		DECLARE @_CategoryMaskingName VARCHAR(150) = (SELECT CategoryMaskingName FROM Con_EditCms_Category WITH(NOLOCK) WHERE Id = @CategoryId)
		DECLARE @_Url VARCHAR(200) = '/' + @_CategoryMaskingName + '/' + @Url + '-' + CAST(@ID AS VARCHAR(18)) + '/'

		UPDATE Con_EditCms_Basic
		SET Url = @_Url
		WHERE Id = @ID
	END

	IF @ID > 0
		AND LEN(@SubCatId) > 0
		AND @SubCatId IS NOT NULL
	BEGIN
		DECLARE @idx INT
		DECLARE @StrTemp VARCHAR(2000)

		SET @idx = 1

		WHILE @idx != 0
		BEGIN
			SET @idx = CharIndex(',', @SubCatId)

			IF @idx != 0
				SET @StrTemp = Left(@SubCatId, @idx - 1)
			ELSE
				SET @StrTemp = @SubCatId

			IF LEN(@StrTemp) > 0
			BEGIN
				INSERT INTO Con_EditCms_BasicSubCategories (
					BasicId
					,SubCategoryId
					)
				VALUES (
					@ID
					,@StrTemp
					)
			END

			SET @SubCatId = Right(@SubCatId, Len(@SubCatId) - @idx)
		END
	END

	DECLARE @isSinglePage BIT

	SET @isSinglePage = 0

	SELECT @isSinglePage = IsSinglePage
	FROM Con_EditCms_Category WITH (NOLOCK)
	WHERE Id = @CategoryId

	IF @isSinglePage = 1
	BEGIN
		DECLARE @pStatus INT

		EXEC dbo.Con_EditCms_ManagePages - 1
			,@ID
			,'Content'
			,1
			,1
			,@LastUpdatedBy
			,@pStatus Out
	END

	IF ltrim(rtrim(@CFId)) NOT IN (
			''
			,'||'
			)
	BEGIN
		--Declare @boolVal Bit = Null    
		--Declare @numericVal Numeric = Null     
		--Declare @decimalVal Decimal = Null     
		--Declare @textVal VarChar(250) = Null    
		--Declare @dateTimeVal DateTime = Null    
		--if @ValType = 1    
		-- Set @boolVal = CONVERT(bit, @Value)    
		--if @ValType = 2    
		-- Set @numericVal = CONVERT(numeric, @Value)     
		--if @ValType = 3    
		-- Set @decimalVal = CONVERT(decimal, @Value)    
		--if @ValType = 4    
		-- Set @textVal = CONVERT(varchar, @Value)    
		--if @ValType = 5    
		-- Set @dateTimeVal = CONVERT(datetime, @Value)     
		INSERT INTO Con_EditCms_OtherInfo (
			BasicId
			,CategoryFieldId
			,TextValue
			,ValueType
			,LastUpdatedTime
			,LastUpdatedBy
			)
		SELECT @ID AS BasicId
			,Id
			,Value
			,ValType
			,@LastUpdatedTime AS LastUpdatedTime
			,@LastUpdatedBy AS LastUpdatedBy
		FROM dbo.Split(@CFId, @ValType, @Value, '|')
			--Values (@ID, @CFId, @boolVal, @numericVal, @decimalVal, @textVal, @dateTimeVal, @ValType,@LastUpdatedTime, @LastUpdatedBy)    
	END
END