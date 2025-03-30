use Avaluo
INSERT INTO Estado (idtabla, descripcion) VALUES
('Rubrica', 'Activa y sin entregar'),
('Rubrica', 'Activa y entregada'),
('Rubrica', 'Entregada'),
('Rubrica', 'No entregada'),
('Usuario','Activo'),
('Usuario','Desactivado'),
('Asignatura', 'Activa'),
('Asignatura', 'Desactivada'),
('Competencia', 'Activa'),
('Competencia', 'Desactivada'),
('MapaCompetencia','Activa y Evaluando'),
('MapaCompetencia','Activa y No Evaluando'),
('Configuracion','Activa'),
('Configuracion','Desactivada'),
('Carrera', 'Activa'),
('Carrera', 'Desactivada'),
('MapaCompetencia','Desactivada')

INSERT INTO metodo_evaluacion (descripcionES, descripcionEN) VALUES
('Examen', 'Exam'),
('Tarea', 'Assignment'),
('Proyecto', 'Project'),
('Presentación', 'Presentation');

INSERT INTO roles (
    Descripcion, 
    EsProfesor, 
    EsSupervisor, 
    EsCoordinadorArea, 
    EsCoordinadorCarrera, 
    EsAdmin, 
    EsAux, 
    VerInformes, 
    VerListaDeRubricas, 
    ConfigurarFechas, 
    VerManejoCurriculum
) VALUES
('Profesor', 1, 0, 0, 0, 0, 0, 0, 0, 0,0 ),
('Administrador', 0, 0, 0, 0, 1, 0, 1, 1, 1, 1),
('Coordinador de Carrera', 0, 0, 0, 1, 0, 0, 1, 1, 1, 1),
('Supervisor', 1, 1, 0, 0, 0, 0, 0, 0, 0, 0),
('Auxiliar', 0, 0, 0, 0, 0, 1, 0, 0, 0, 0),
('Prueba', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
('Coordinador de Area',0,0,1,0,0,0,1,1,1,1),
('Coordinador de Carrera y Profesor', 1, 0,0,1,0,0,1,1,1,1),
('Coordinador de Area y Profesor', 1, 0,0,1,0,0,1,1,1,1),
('Coordinador de Carrera y Supervisor', 1, 1,0,1,0,0,1,1,1,1),
('Coordinador de Area y Supervisor', 1, 1,1,0,0,0,1,1,1,1)

INSERT INTO tipo_competencia (Nombre) VALUES
('Específica'),
('General'),
('Negocios')

INSERT INTO [dbo].[areas] ([Descripcion], [IdCoordinador], [FechaCreacion], [UltimaEdicion])  VALUES  
( 'Área de Ingeniería', NULL, GETDATE(), NULL),
( 'Área de Ciencias Básicas', NULL, GETDATE(), NULL), 
( 'Área de Negocios', NULL, GETDATE(), NULL),
( 'Área de Idiomas', NULL, GETDATE(), NULL),
( 'Área de Humanidades', NULL, GETDATE(), NULL)

INSERT INTO Asignaturas (Creditos, Codigo, Nombre, FechaCreacion, UltimaEdicion, IdEstado, ProgramaAsignatura, Syllabus, IdArea) 
VALUES 
(4, 'AHC109', 'REDACCION', GETDATE(), GETDATE(), 7, 'Fundamentos de la Escritura Académica', 'https://syllabus.universidad.edu/redaccion.pdf', 5),
(5, 'CBM101', 'ALGEBRA Y GEOMETRIA ANALITICA', GETDATE(), GETDATE(), 7, 'Conceptos básicos de álgebra y geometría', 'https://syllabus.universidad.edu/algebra.pdf', 2),
(2, 'CBA106', 'AMBIENTE Y CULTURA', GETDATE(), GETDATE(), 7, 'Estudio de la relación entre el ambiente y la sociedad', 'Por definir', 2),
(2, 'CCP103', 'CREATIVIDAD', GETDATE(), GETDATE(), 7, 'Desarrollo de habilidades creativas', 'https://syllabus.universidad.edu/creatividad.pdf', 5),
(2, 'IDS207', 'INTRODUCCION A LA INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Conceptos básicos de ingeniería de software', 'https://syllabus.universidad.edu/intro_software.pdf', 1),
(4, 'AHC110', 'ARGUMENTACION LINGUISTICA', GETDATE(), GETDATE(), 7, 'Estrategias para la argumentación efectiva', 'https://syllabus.universidad.edu/argumentacion.pdf', 5),
(5, 'CBM102', 'CALCULO DIFERENCIAL', GETDATE(), GETDATE(), 7, 'Introducción a derivadas y aplicaciones', 'https://syllabus.universidad.edu/calculo_dif.pdf', 2),
(4, 'IDS323', 'TECNICAS FUNDAMENTALES DE INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Principales metodologías de desarrollo', 'Por definir', 1),
(2, 'ING102', 'INTRODUCCION A LA PROGRAMACION', GETDATE(), GETDATE(), 7, 'Principios básicos de programación', 'https://syllabus.universidad.edu/intro_prog.pdf', 1),
(4, 'CBF210', 'FISICA MECANICA I', GETDATE(), GETDATE(), 7, 'Conceptos de cinemática y dinámica', 'https://syllabus.universidad.edu/fisica_mec.pdf', 2),
(5, 'CBM201', 'CALCULO INTEGRAL', GETDATE(), GETDATE(), 7, 'Integrales y aplicaciones', 'https://syllabus.universidad.edu/calculo_int.pdf', 2),
(4, 'IDS202', 'TECNOLOGIA DE OBJETOS', GETDATE(), GETDATE(), 7, 'Introducción a la programación orientada a objetos', 'https://syllabus.universidad.edu/tecn_objetos.pdf', 1),
(3, 'IDS340', 'DESARROLLO DE SOFTWARE I', GETDATE(), GETDATE(), 7, 'Fundamentos del desarrollo de software', 'https://syllabus.universidad.edu/desarrollo_sw1.pdf', 1),
(5, 'CBM202', 'CALCULO VECTORIAL', GETDATE(), GETDATE(), 7, 'Cálculo en varias variables', 'Por definir', 2),
(3, 'IDS341', 'DESARROLLO DE SOFTWARE II', GETDATE(), GETDATE(), 7, 'Aplicaciones avanzadas en desarrollo de software', 'https://syllabus.universidad.edu/desarrollo_sw2.pdf', 1),
(4, 'IDS208', 'TEAM BUILDING', GETDATE(), GETDATE(), 7, 'Habilidades para el trabajo en equipo', 'https://syllabus.universidad.edu/team_building.pdf', 1),
(5, 'CBM208', 'ALGEBRA LINEAL', GETDATE(), GETDATE(), 7, 'Espacios vectoriales y transformaciones lineales', 'Por definir', 2),
(3, 'IDS311', 'PROCESO DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Ciclo de vida del software', 'https://syllabus.universidad.edu/proceso_software.pdf', 1),
(4, 'IDS324', 'INGENIERIA DE REQUERIMIENTOS DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Gestión y documentación de requisitos', 'https://syllabus.universidad.edu/req_sw.pdf', 1),
(1, 'IDS343L','LABORATORIO ESTRUCTURAS DE DATOS Y ALGORITMOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'INS377', 'BASES DE DATOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'INS377L', 'LABORATORIO BASES DE DATOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(3, 'IDS345', 'DESARROLLO DE SOFTWARE III', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'IDS345L', 'LABORATORIO DE DESARROLLO DE SOFTWARE III', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(3, 'IDS346', 'MODELOS Y METODOS DE LA INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'IDS329', 'INGENIERIA DE FACTORES HUMANOS', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'INS380', 'BASES DE DATOS II', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'INS380L', 'LABORATORIO DE BASES DE DATOS II', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1)


INSERT INTO Competencia (Nombre, Acron, Titulo, DescripcionES, DescripcionEN, id_tipo, id_estado, FechaCreacion, UltimaEdicion) VALUES
('Student Outcome 1', 'SO1', 'Resolución de problemas complejos de ingeniería', 
 'Capacidad para identificar, formular y resolver problemas complejos de ingeniería aplicando principios de ingeniería, ciencia y matemáticas.', 
 'Ability to identify, formulate, and solve complex engineering problems by applying principles of engineering, science, and mathematics.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 2', 'SO2', 'Diseño de soluciones de ingeniería', 
 'Capacidad para aplicar el diseño de ingeniería para producir soluciones que satisfagan necesidades específicas, considerando factores de salud pública, seguridad y bienestar, así como factores globales, culturales, sociales, ambientales y económicos.', 
 'Ability to apply engineering design to produce solutions that meet specified needs with consideration of public health, safety, and welfare, as well as global, cultural, social, environmental, and economic factors.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 3', 'SO3', 'Comunicación efectiva', 
 'Capacidad para comunicarse de manera efectiva con diferentes audiencias.', 
 'Ability to communicate effectively with a range of audiences.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 4', 'SO4', 'Responsabilidad ética y profesionalismo', 
 'Capacidad para reconocer responsabilidades éticas y profesionales en situaciones de ingeniería y hacer juicios informados, considerando el impacto de las soluciones en contextos globales, económicos, ambientales y sociales.', 
 'Ability to recognize ethical and professional responsibilities in engineering situations and make informed judgments, considering the impact of engineering solutions in global, economic, environmental, and societal contexts.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 5', 'SO5', 'Trabajo en equipo y liderazgo', 
 'Capacidad para trabajar eficazmente en un equipo cuyos miembros proporcionan liderazgo, crean un entorno colaborativo e inclusivo, establecen objetivos, planifican tareas y cumplen con los objetivos.', 
 'Ability to function effectively on a team whose members provide leadership, create a collaborative and inclusive environment, establish goals, plan tasks, and meet objectives.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 6', 'SO6', 'Experimentación y análisis de datos', 
 'Capacidad para desarrollar y llevar a cabo experimentos adecuados, analizar e interpretar datos y usar el juicio de ingeniería para sacar conclusiones.', 
 'Ability to develop and conduct appropriate experimentation, analyze and interpret data, and use engineering judgment to draw conclusions.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 7', 'SO7', 'Aprendizaje continuo y adaptación', 
 'Capacidad para adquirir y aplicar nuevos conocimientos según sea necesario, utilizando estrategias de aprendizaje apropiadas.', 
 'Ability to acquire and apply new knowledge as needed, using appropriate learning strategies.', 
 1, 9, GETDATE(), GETDATE());
 

 INSERT INTO pi (Nombre, SO_id, descripcionEN, descripcionES) VALUES
('PI 1', 1, 'Define the problem', 'Define el problema'),
('PI 2', 1, 'Determine the causes of the problem', 'Determina las causas del problema'),
('PI 3', 1, 'Propose solutions to the problem by applying principles of engineering, science, and/or mathematics', 'Propone soluciones al problema aplicando principios de ingeniería, ciencia y/o matemáticas'),
('PI 4', 1, 'Justifies the selected solution', 'Justifica la solución seleccionada'),
('PI 1', 2, 'Identifies needs and converts them into objectives, criteria and design restrictions', 'Identifica necesidades y las convierte en objetivos, criterios y restricciones de diseño'),
('PI 2', 2, 'Generates alternatives based on engineering sciences, social sciences, economics, among others', 'Genera alternativas basadas en ciencias de la ingeniería, ciencias sociales, economía, entre otras'),
('PI 3', 2, 'Selects the best alternative', 'Selecciona la mejor alternativa'),
('PI 4', 2, 'Creates specifications, prototypes or other means of design communication', 'Crea especificaciones, prototipos u otros medios de comunicación del diseño'),
('PI 1', 3, 'Writes reports based on standard formats adapted to the target audience', 'Redacta informes basados en formatos estándar adaptados al público objetivo'),
('PI 2', 3, 'Presents the content orally in his/her own words and adapts it to the audience present', 'Presenta el contenido oralmente con sus propias palabras y lo adapta a la audiencia presente'),
('PI 3', 3, 'Complements the information using graphs, diagrams, among others, according to the audience', 'Complementa la información utilizando gráficos, diagramas, entre otros, según la audiencia'),
('PI 4', 3, 'Provides the information with sustainable evidence', 'Proporciona la información con evidencia sustentable'),
('PI 1', 4, 'Develops solutions in accordance with current reality, taking into account ethical and professional responsibility', 'Desarrolla soluciones de acuerdo con la realidad actual, tomando en cuenta la responsabilidad ética y profesional'),
('PI 2', 4, 'Assesses the consequences of the impact of engineering decisions in global, regional and local contexts (economic, environmental and social)', 'Evalúa las consecuencias del impacto de las decisiones de ingeniería en contextos globales, regionales y locales (económico, ambiental y social)'),
('PI 3', 4, 'Recognizes the copyright in the particular solutions developed', 'Reconoce los derechos de autor en las soluciones particulares desarrolladas'),
('PI 1', 5, 'Plans strategies to meet objectives', 'Planifica estrategias para cumplir objetivos'),
('PI 2', 5, 'Interacts and communicates with team members, open to the opinions of others', 'Interactúa y se comunica con los miembros del equipo, abierto a las opiniones de otros'),
('PI 3', 5, 'Identifies and fulfills his role as a member of the work team to achieve the objectives', 'Identifica y cumple su rol como miembro del equipo de trabajo para lograr los objetivos'),
('PI 1', 6, 'Plan or design the experiment', 'Planifica o diseña el experimento'),
('PI 2', 6, 'Conduct the experiment', 'Realiza el experimento'),
('PI 3', 6, 'Analyze and interpret the results', 'Analiza e interpreta los resultados'),
('PI 4', 6, 'Discuss the results obtained from the experiment', 'Discute los resultados obtenidos del experimento'),
('PI 1', 7, 'Need to acquire new knowledge', 'Necesidad de adquirir nuevos conocimientos'),
('PI 2', 7, 'Relationship between knowledge and learning strategy', 'Relación entre conocimiento y estrategia de aprendizaje'),
('PI 3', 7, 'Obtaining new concepts', 'Obtención de nuevos conceptos');

INSERT INTO mapa_competencias (Id_Asignatura, Id_Competencia, Id_Estado) VALUES 
(13,3,11),
(13,1,11),
(13,5,11),
(15,2,11),
(15,6,11),
(20,7,11),
(20,3,11),
(8,4,11),
(8,3,11),
(16,2,11),
(16,1,11),
(18,3,11),
(18,2,11)

INSERT INTO so_evaluacion (Id_SO, Id_MetodoEvaluacion) VALUES
(1, 2),
(1, 3),
(1, 4),
(2, 1),
(2, 2),
(2, 3),
(2, 4),
(3, 1),
(3, 2),
(3, 4),
(4, 1),
(4, 2),
(5, 2),
(5, 3),
(6, 1),
(6, 2),
(6, 3),
(7, 1),
(7, 3),
(7, 4);


INSERT INTO [dbo].[carreras]
           ([IdArea]
           ,[Año]
           ,[NombreCarrera]
           ,[IdCoordinadorCarrera]
           ,[PEOs]
           ,[FechaCreacion]
           ,[UltimaEdicion]
           ,[IdEstado])
     VALUES
           (1,2020,'Ingeniería de Software',null,
		   'Ser líderes en organizaciones locales e internacionales, ejerciendo la ingeniería industrial en un marco de integridad, excelencia y responsabilidad social.
			Generar soluciones integradas y sustentables para la industria considerando la actualidad y el contexto global.
			Lograr el desarrollo profesional y el crecimiento del conocimiento a través de certificaciones internacionales, estudios de posgrado, experiencia laboral y autoaprendizaje.',
			GETDATE(),null,15),
			(1,2020,'Ingeniería en Sistemas',null,
		   'Ser líderes en organizaciones locales e internacionales, ejerciendo la ingeniería industrial en un marco de integridad, excelencia y responsabilidad social.
			Generar soluciones integradas y sustentables para la industria considerando la actualidad y el contexto global.
			Lograr el desarrollo profesional y el crecimiento del conocimiento a través de certificaciones internacionales, estudios de posgrado, experiencia laboral y autoaprendizaje.',
			GETDATE(),null,15);



INSERT INTO [dbo].[asignatura_carrera] ([Id_Asignatura],[Id_Carrera])
     VALUES
           (5,1),
           (8,1),
           (12,1), 
           (13,1), 
           (15,1),
           (12,2),
           (13,2),
           (15,2),
           (16,1),
           (18,1),
           (19,1), 
           (19,2),
           (20,1),
           (21,1), 
           (22,1), 
           (23,1), 
           (24,1), 
           (21,2), 
           (22,2), 
           (23,2), 
           (24,2)




-- Datos de prueba para edificios
INSERT INTO [dbo].[edificios] 
( [Id_Area], [Nombre], [FechaCreacion], [UltimaEdicion], [Acron], [Id_Estado], [Ubicacion]) 
VALUES
( 1, 'Edificio de Ingeniería', GETDATE(), NULL, 'ING', 1, 'Campus Norte'),
( 2, 'Edificio de Ciencias', GETDATE(), NULL, 'SCI', 1, 'Campus Central');


-- Datos de prueba para aula
INSERT INTO [dbo].[aula] 
( [Descripcion], [FechaCreacion], [UltimaEdicion], [Id_Edificio], [Id_Estado]) 
VALUES
( 'GC301', GETDATE(), NULL, 1, 1),
( 'GC311', GETDATE(), NULL, 2, 1);








-- Datos de prueba para tipo_informe
INSERT INTO [dbo].[tipo_informe] 
( [Descripcion]) 
VALUES
('Desempeño de SO'),
( 'Reporte Anual');





INSERT INTO mapa_competencias (Id_Asignatura, Id_Competencia, Id_Estado)
SELECT 
    ac.Id_Asignatura,
    comp.Id_Competencia,
    17 AS Id_Estado
FROM (
    SELECT DISTINCT Id_Asignatura FROM asignatura_carrera
) ac
CROSS JOIN (
    SELECT v.number AS Id_Competencia
    FROM (VALUES (1), (2), (3), (4), (5), (6), (7)) AS v(number)
) comp
WHERE NOT EXISTS (
    SELECT 1 
    FROM mapa_competencias mc
    WHERE mc.Id_Asignatura = ac.Id_Asignatura
      AND mc.Id_Competencia = comp.Id_Competencia
);