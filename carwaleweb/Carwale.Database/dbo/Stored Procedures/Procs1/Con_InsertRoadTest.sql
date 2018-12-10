IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertRoadTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertRoadTest]
GO

	CREATE PROCEDURE [dbo].[Con_InsertRoadTest] 

@RTID NUMERIC,
@CarModelID NUMERIC,
@CarVersionID NUMERIC,
@Title VARCHAR(250),
@DisplayDate DATETIME,
@AuthorName VARCHAR(100),
@Pros VARCHAR(500),
@Cons VARCHAR(500),
@EntryDate DATETIME,
@IsType SMALLINT,
@Description VARCHAR(2000),
@ID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @RTID = -1
		BEGIN
		INSERT INTO Con_RoadTest 
		(CarModelID,CarVersionID,Title,DisplayDate,AuthorName,Pros,Cons,EntryDate,IsType,Description) 
		VALUES 
		(@CarModelID,@CarVersionID,@Title,@DisplayDate,@AuthorName,@Pros,@Cons,@EntryDate,@IsType,@Description)
		SET @ID = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_RoadTest
		SET CarModelID = @CarModelID, CarVersionID=@CarVersionID, Title=@Title,
		DisplayDate = @DisplayDate, AuthorName=@AuthorName, Pros=@Pros,
		Cons=@Cons, IsType=@IsType , Description=@Description
		Where ID = @RTID
		SET @ID = @RTID 
		END
	
END









