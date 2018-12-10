IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SearchForums]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SearchForums]
GO

	-- This procedure is to enter forum searches

-- Modified By : Ravi Koshal
-- Description : IsModerated condition added for retrieval of forum(s)/forumthread(s) for the required search query.
--Modified by: Manish Chourasiya on 24-08-2015 added with(nolock) keyword wherever not found.

CREATE  PROCEDURE [dbo].[SearchForums]
	@SearchTerm		VARCHAR(500),
	@SearchDateTime	DATETIME, 
	@SearchType		VARCHAR(100),
	@SearchId		NUMERIC OUTPUT,
	@Results		NUMERIC OUTPUT
 AS
	DECLARE @ActualSearchString  AS	VARCHAR(100)
BEGIN
	-- check if it's a special search...
	IF @SearchType = 'PostsBy' OR @SearchType = 'ThreadsBy' 
	BEGIN
		SET @ActualSearchString = @SearchType + ':' + @SearchTerm	
	END
	ELSE
	BEGIN
		SET @ActualSearchString = @SearchTerm	
	END

	-- Insert search terms
	INSERT INTO ForumSearches(SearchTerm, SearchDateTime) VALUES(@ActualSearchString,@SearchDateTime)

	-- Fetch search id
	SET @SearchId=SCOPE_IDENTITY() 
	
	-- Now find and insert the results
	
	IF @SearchType = 'ThreadsBy'
	BEGIN
		INSERT INTO ForumSearchResults(SearchId, ForumThreadId, IsTitleMatch) 
		SELECT @SearchId, ID, 1 FROM Forums F WITH(NOLOCK) WHERE IsActive=1 AND CustomerId = @SearchTerm AND ID IN (SELECT ForumId 
					FROM ForumThreads WITH(NOLOCK) WHERE ForumId=F.Id AND IsModerated = 1)
	END
	ELSE
	BEGIN
		IF @SearchType = 'PostsBy'
		BEGIN
			INSERT INTO ForumSearchResults(SearchId, ForumThreadId, IsTitleMatch) 
			SELECT @SearchId, FT.ID, 0 
				FROM ForumThreads FT WITH(NOLOCK) , Forums F  WITH(NOLOCK)
				WHERE FT.customerId=@SearchTerm AND FT.IsActive=1 AND F.IsActive=1 AND F.Id=FT.ForumId AND FT.IsModerated = 1
		END
		ELSE
		BEGIN
			IF @SearchType = 'ByDate'
			BEGIN
				INSERT INTO ForumSearchResults(SearchId, ForumThreadId, IsTitleMatch) 
				SELECT @SearchId, ID, 1 FROM Forums F WITH(NOLOCK) WHERE ID IN ( SELECT ForumId 
					FROM ForumThreads WITH(NOLOCK) WHERE ForumId=F.Id AND MsgDateTime >= @SearchTerm AND IsActive=1 AND IsModerated = 1) AND IsActive=1
			END
			ELSE
			BEGIN
				INSERT INTO ForumSearchResults(SearchId, ForumThreadId, IsTitleMatch) SELECT @SearchId, ID, 1 FROM Forums WITH(NOLOCK) WHERE IsActive=1 AND	IsModerated = 1 AND Topic LIKE '%' + @SearchTerm + '%'
				IF @SearchType <> 'TitlesOnly'
				BEGIN
					INSERT INTO ForumSearchResults(SearchId, ForumThreadId, IsTitleMatch) 
					SELECT @SearchId, ID, 0 FROM Forums F WITH(NOLOCK) WHERE ID IN ( SELECT ForumId 
						FROM ForumThreads WITH(NOLOCK) WHERE ForumId=F.Id AND Message LIKE '%' + @SearchTerm + '%' AND IsActive=1 AND IsModerated = 1) AND IsActive = 1  
						AND F.Id NOT IN (SELECT ForumThreadID FROM ForumSearchResults WITH(NOLOCK)
						WHERE IsTitleMatch=1 AND SearchId=@SearchId)
				END
			END
		END
	END	

	SELECT @Results=COUNT(ForumThreadId) FROM ForumSearchResults  WITH(NOLOCK) WHERE SearchId=@SearchId
END
