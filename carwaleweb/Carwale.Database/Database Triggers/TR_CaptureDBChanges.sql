
---- Created By: Manish on 14-07-2014 for capturing logs of the changes in the DB
CREATE  TRIGGER [TR_CaptureDBChanges]
ON DATABASE
FOR  CREATE_PROCEDURE, 
     ALTER_PROCEDURE, 
	 DROP_PROCEDURE,
     CREATE_TABLE, 
     ALTER_TABLE, 
	 DROP_TABLE,
     CREATE_TRIGGER,
     ALTER_TRIGGER,
     DROP_TRIGGER,
	 CREATE_VIEW,
     ALTER_VIEW,
     DROP_VIEW,
	 CREATE_FUNCTION,
     ALTER_FUNCTION,
     DROP_FUNCTION,
	 CREATE_INDEX,
     ALTER_INDEX,
     DROP_INDEX,
	 CREATE_SCHEMA,
     ALTER_SCHEMA,
     DROP_SCHEMA,
	 CREATE_SYNONYM,
     DROP_SYNONYM,
	 CREATE_TYPE,
     DROP_TYPE
AS
BEGIN
DECLARE @ed XML
SET @ed = EVENTDATA()

 DECLARE 
        @ip VARCHAR(32) =
        (
            SELECT client_net_address
                FROM sys.dm_exec_connections
                WHERE session_id = @@SPID
        );

		INSERT INTO DBChangesLogs ( EventDate,
									DBName,
									SchemaName,
									ObjectType,
									EventType,
									ObjectName,
									SQLScript,
									HostName,
									IPAddress,
									LoginName,
									ProgramName) 
					VALUES
								(
									GetDate(),
									@ed.value('(/EVENT_INSTANCE/DatabaseName)[1]', 'varchar(256)'),
									@ed.value('(/EVENT_INSTANCE/SchemaName)[1]',  'VARCHAR(255)'), 
									@ed.value('(/EVENT_INSTANCE/ObjectType)[1]', 'varchar(50)'),
									@ed.value('(/EVENT_INSTANCE/EventType)[1]', 'varchar(100)'),
									@ed.value('(/EVENT_INSTANCE/ObjectName)[1]', 'varchar(256)'),
									@ed.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'VARCHAR(MAX)'),
									HOST_NAME(), 
									@ip,
									@ed.value('(/EVENT_INSTANCE/LoginName)[1]', 'varchar(256)'),
									PROGRAM_NAME()
								) 

END



