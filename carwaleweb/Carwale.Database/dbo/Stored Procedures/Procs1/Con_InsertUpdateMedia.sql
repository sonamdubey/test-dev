IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateMedia]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateMedia]
GO

	CREATE procedure [dbo].[Con_InsertUpdateMedia]
@MID NUMERIC,
@PublisherId NUMERIC,
@Online BIT,
@Url VARCHAR(200),
@Title VARCHAR(250),
@PublishDate DATETIME,
@CreatedBy VARCHAR(100),
@Desc VARCHAR(500),
@EntryDate DATETIME,
@ID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @MID = -1
		BEGIN
		INSERT INTO Media 
		(PublisherId, Online, Url, Title, PublishDate, CreatedBy, SmallDescription, CreatedOn)
		VALUES 
		(@PublisherId, @Online, @Url, @Title, @PublishDate, @CreatedBy, @Desc, @EntryDate)
		SET @ID = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Media
		SET PublisherId = @PublisherId, Online=@Online, Url=@Url, Title=@Title,
		PublishDate = @PublishDate, CreatedBy = @CreatedBy, SmallDescription = @Desc
		WHERE ID = @MID
		SET @ID = @MID
		END
	
END