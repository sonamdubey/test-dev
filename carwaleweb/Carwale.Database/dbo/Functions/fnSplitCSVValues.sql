IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnSplitCSVValues]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fnSplitCSVValues]
GO

	


-- ============================================= 
-- Author:    Avishkar
-- Create date: 19-Dec-12 
-- Description:  Return the csv values in single column list
-- ============================================= 
-- Table-valued user-defined function - TVF  
CREATE FUNCTION [dbo].[fnSplitCSVValues] ( @NumberList varchar(4096))  
RETURNS @SplitList TABLE (    ListMember varchar(25) )  
AS  
BEGIN  
      DECLARE @Pointer int, @ListMember varchar(25)  
      SET @NumberList = LTRIM(RTRIM(@NumberList))  
      IF (RIGHT(@NumberList, 1) != ',')  
         SET @NumberList=@NumberList+ ','  
      SET @Pointer = CHARINDEX(',', @NumberList, 1)  
      IF REPLACE(@NumberList, ',', '') <> ''  
      BEGIN  
            WHILE (@Pointer > 0)  
            BEGIN  
                  SET @ListMember = LTRIM(RTRIM(LEFT(@NumberList, @Pointer - 1)))  
                  IF (@ListMember <> '')  
                  INSERT INTO @SplitList  
                        VALUES (@ListMember)
                  SET @NumberList = RIGHT(@NumberList, LEN(@NumberList) - @Pointer)  
                  SET @Pointer = CHARINDEX(',', @NumberList, 1)  
            END  
      END    
      RETURN  
END  

