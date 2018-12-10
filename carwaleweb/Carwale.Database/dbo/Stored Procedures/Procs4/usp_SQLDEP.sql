IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_SQLDEP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_SQLDEP]
GO

	--exec usp_SQLDEP 'GetLeadScoreParameters'
CREATE PROC [dbo].[usp_SQLDEP]
@referencing_entity AS sysname

AS

WITH ObjectDepends(entity_name,referenced_schema, referenced_entity, referenced_id,level)
AS
(
    SELECT entity_name = 
       CASE referencing_class
          WHEN 1 THEN OBJECT_NAME(referencing_id)
          WHEN 12 THEN (SELECT t.name FROM sys.triggers AS t 
                       WHERE t.object_id = sed.referencing_id)
          WHEN 13 THEN (SELECT st.name FROM sys.server_triggers AS st
                       WHERE st.object_id = sed.referencing_id) COLLATE database_default
       END
    ,referenced_schema_name
    ,referenced_entity_name
    ,referenced_id
    ,0 AS level 
    FROM sys.sql_expression_dependencies AS sed 
    WHERE OBJECT_NAME(referencing_id) = @referencing_entity 
UNION ALL
    SELECT entity_name = 
       CASE sed.referencing_class
          WHEN 1 THEN OBJECT_NAME(sed.referencing_id)
          WHEN 12 THEN (SELECT t.name FROM sys.triggers AS t 
                       WHERE t.object_id = sed.referencing_id)
          WHEN 13 THEN (SELECT st.name FROM sys.server_triggers AS st
                       WHERE st.object_id = sed.referencing_id) COLLATE database_default
       END
    ,sed.referenced_schema_name
    ,sed.referenced_entity_name
    ,sed.referenced_id
    ,level + 1   
    FROM ObjectDepends AS o
    JOIN sys.sql_expression_dependencies AS sed ON sed.referencing_id = o.referenced_id
)

SELECT referenced_entity
FROM ObjectDepends;

