EXEC sp_addmessage 
    @msgnum = 50001, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Nickname already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50002, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Email already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50003, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Elo must be between 0 and 3000', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50004, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Minimum number of players cannot be greater than Maximum number of players', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50005, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Minimum ELO cannot be greater than Maximum ELO', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50006, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'End inscription date must be in the future', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50007, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player does not exist', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50008, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player''s Elo is not in the range of the tournament''s Elo requirements.', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50009, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Age Category doesn''t exist', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50010, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Tournament not found', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50011, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Tournament is full', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50012, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Tournament inscription is closed', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50013, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player not found', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50014, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player ELO out of range', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50015, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player is not in the age range of the tournament''s age category requirements.', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50016, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player is already registered to the tournament.', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50017, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = '%s', -- Error message -- player not egible
    @lang = 'us_english'; -- L anguage
GO;