IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BindCarModelRoots]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BindCarModelRoots]
GO

	
-- ============================================= 
-- Author: Prashant Vishe    
-- Create date: <29 Jan 2013> 
-- Description:   For Binding Car model Roots
-- ============================================= 
CREATE PROCEDURE [dbo].[BindCarModelRoots]
 @Id     NUMERIC = NULL, 
 @MakeId NUMERIC = NULL 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON; 

      -- Insert statements for procedure here 
      IF @MakeId = 0 
        BEGIN 
            SET @MakeId = NULL 
        END 

      SELECT CR.makeid, 
             CM.name AS Make, 
             rootname, 
             CR.rootid 
      FROM   carmodelroots CR  WITH (NOLOCK)
             INNER JOIN carmakes CM  WITH (NOLOCK)
                     ON CM.id = CR.makeid 
      WHERE  ( CR.rootid = @Id 
                OR @Id IS NULL ) 
             AND ( CR.makeid = @MakeId 
                    OR @MakeId IS NULL ) 
      ORDER  BY CM.name ASC 
  END 

