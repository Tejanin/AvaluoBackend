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
('MapaCompetencia','Evaluando'),
('MapaCompetencia','No Evaluando'),
('Configuracion','Activa'),
('Configuracion','Desactivada'),
('Carrera', 'Activa'),
('Carrera', 'Desactivada')

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
('Profesor', 1, 0, 0, 0, 0, 0, 1, 1, 0, 1),
('Administrador', 0, 0, 0, 0, 1, 0, 1, 1, 1, 1),
('Coordinador', 0, 0, 1, 1, 0, 0, 1, 1, 1, 1),
('Supervisor', 0, 1, 0, 0, 0, 0, 1, 1, 1, 1),
('Auxiliar', 0, 0, 0, 0, 0, 1, 0, 0, 0, 0),
('Prueba', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

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


-- Datos de prueba para desempeno
INSERT INTO [dbo].[desempeno] 
([Id], [Id_SO], [IdPI], [Id_Asignatura], [Satisfactorio], [Trimestre], [Año], [Porcentaje]) 
VALUES
(1, 1, 1, 1, 1, '1', 2024, 85.5),
(2, 2, 2, 2, 0, '2', 2024, 70.0),
(3, 3, 3, 3, 1, '1', 2024, 88.0),
(4, 4, 4, 4, 0, '2', 2024, 65.2),
(5, 5, 5, 5, 1, '3', 2024, 92.4),
(6, 6, 6, 6, 1, '1', 2023, 80.1),
(7, 7, 7, 7, 0, '2', 2023, 69.8),
(8, 1, 2, 8, 1, '3', 2023, 75.0),
(9, 2, 3, 9, 1, '1', 2022, 85.7),
(10, 3, 4, 10, 0, '2', 2022, 60.5),
(11, 4, 5, 11, 1, '3', 2022, 90.2),
(12, 5, 6, 12, 1, '1', 2021, 78.4),
(13, 6, 7, 13, 0, '2', 2021, 62.3),
(14, 7, 1, 14, 1, '3', 2021, 88.9),
(15, 1, 3, 15, 1, '1', 2020, 94.6),
(16, 2, 4, 16, 0, '2', 2020, 55.1),
(17, 3, 5, 17, 1, '3', 2020, 79.5),
(18, 4, 6, 18, 1, '1', 2024, 83.2),
(19, 5, 7, 19, 0, '2', 2024, 66.7),
(20, 6, 1, 20, 1, '3', 2024, 95.0),
(21, 7, 2, 21, 1, '1', 2023, 87.3),
(22, 1, 4, 22, 0, '2', 2023, 72.5),
(23, 2, 5, 23, 1, '3', 2023, 91.1),
(24, 3, 6, 24, 1, '1', 2022, 79.9),
(25, 4, 7, 25, 0, '2', 2022, 64.8),
(26, 5, 1, 26, 1, '3', 2022, 88.6),
(27, 6, 2, 27, 1, '1', 2021, 90.0),
(28, 7, 3, 28, 0, '2', 2021, 68.2),
(29, 1, 5, 10, 1, '1', 2024, 91.2),
(30, 2, 6, 12, 0, '2', 2024, 67.3),
(31, 3, 7, 14, 1, '3', 2024, 89.8),
(32, 4, 1, 16, 0, '1', 2023, 61.5),
(33, 5, 2, 18, 1, '2', 2023, 77.9),
(34, 6, 3, 20, 1, '3', 2023, 85.4),
(35, 7, 4, 22, 0, '1', 2022, 69.1),
(36, 1, 5, 24, 1, '2', 2022, 90.5),
(37, 2, 6, 26, 1, '3', 2022, 81.3),
(38, 3, 7, 28, 0, '1', 2021, 72.4),
(39, 4, 1, 3, 1, '2', 2021, 88.7),
(40, 5, 2, 2, 1, '3', 2021, 92.1),
(41, 6, 3, 5, 0, '1', 2020, 65.4),
(42, 7, 4, 8, 1, '2', 2020, 79.6),
(43, 1, 6, 12, 1, '3', 2024, 94.2),
(44, 2, 7, 16, 0, '1', 2023, 60.9),
(45, 3, 1, 20, 1, '2', 2023, 87.5),
(46, 4, 2, 24, 1, '3', 2023, 90.3),
(47, 5, 3, 28, 0, '1', 2022, 73.4),
(48, 6, 4, 17, 1, '2', 2022, 85.9),
(49, 7, 5, 1, 1, '3', 2022, 92.5),
(50, 1, 7, 3, 0, '1', 2021, 68.7),
(51, 2, 1, 6, 1, '2', 2021, 84.3),
(52, 3, 2, 9, 1, '3', 2021, 89.1),
(53, 4, 3, 13, 0, '1', 2020, 66.8),
(54, 5, 4, 17, 1, '2', 2020, 78.5),
(55, 6, 5, 21, 1, '3', 2020, 91.7),
(56, 7, 6, 25, 0, '1', 2024, 64.5),
(57, 1, 3, 23, 1, '2', 2024, 86.2),
(58, 2, 4, 1, 1, '3', 2024, 93.1),
(59, 3, 5, 2, 0, '1', 2023, 71.3),
(60, 4, 6, 4, 1, '2', 2023, 80.4),
(61, 5, 7, 7, 1, '3', 2023, 95.0),
(62, 6, 1, 10, 0, '1', 2022, 63.2),
(63, 7, 2, 15, 1, '2', 2022, 90.9),
(64, 1, 3, 19, 1, '3', 2022, 85.7),
(65, 2, 4, 23, 0, '1', 2021, 70.8),
(66, 3, 5, 27, 1, '2', 2021, 87.1),
(67, 4, 6, 18, 1, '3', 2021, 94.5),
(68, 5, 7, 3, 0, '1', 2020, 69.2),
(69, 6, 1, 6, 1, '2', 2020, 83.8),
(70, 7, 2, 9, 1, '3', 2020, 90.2),
(71, 1, 4, 1, 1, '1', 2024, 82.5),
(72, 2, 5, 1, 0, '2', 2024, 68.3),
(73, 3, 6, 1, 1, '3', 2024, 90.1),
(74, 4, 2, 2, 1, '1', 2024, 85.9),
(75, 5, 3, 2, 0, '2', 2024, 72.4),
(76, 6, 7, 2, 1, '3', 2024, 88.7),
(77, 7, 1, 3, 1, '1', 2024, 92.3),
(78, 1, 2, 3, 0, '2', 2024, 69.1),
(79, 2, 3, 3, 1, '3', 2024, 86.8),
(80, 3, 5, 4, 1, '1', 2024, 81.6),
(81, 4, 6, 4, 0, '2', 2024, 70.5),
(82, 5, 7, 4, 1, '3', 2024, 89.2),
(83, 6, 1, 5, 1, '1', 2024, 94.0),
(84, 7, 2, 5, 0, '2', 2024, 65.7),
(85, 1, 3, 5, 1, '3', 2024, 83.5),
(86, 2, 5, 6, 1, '1', 2024, 91.8),
(87, 3, 6, 6, 0, '2', 2024, 67.2),
(88, 4, 7, 6, 1, '3', 2024, 85.3),
(89, 5, 1, 7, 1, '1', 2024, 93.6),
(90, 6, 2, 7, 0, '2', 2024, 71.1),
(91, 7, 3, 7, 1, '3', 2024, 88.0),
(92, 1, 5, 8, 1, '1', 2024, 82.7),
(93, 2, 6, 8, 0, '2', 2024, 69.4),
(94, 3, 7, 8, 1, '3', 2024, 91.5),
(95, 4, 2, 9, 1, '1', 2024, 86.3),
(96, 5, 3, 9, 0, '2', 2024, 74.9),
(97, 6, 5, 9, 1, '3', 2024, 87.1),
(98, 7, 6, 10, 1, '1', 2024, 95.2),
(99, 1, 7, 10, 0, '2', 2024, 68.5),
(100, 2, 1, 10, 1, '3', 2024, 89.9),
(101, 3, 2, 11, 1, '1', 2024, 90.4),
(102, 4, 3, 11, 0, '2', 2024, 66.9),
(103, 5, 5, 11, 1, '3', 2024, 83.0),
(104, 6, 6, 12, 1, '1', 2024, 91.2),
(105, 7, 7, 12, 0, '2', 2024, 72.0),
(106, 1, 1, 12, 1, '3', 2024, 86.5),
(107, 2, 2, 13, 1, '1', 2024, 92.8),
(108, 3, 3, 13, 0, '2', 2024, 69.7),
(109, 4, 5, 13, 1, '3', 2024, 87.9),
(110, 5, 6, 14, 1, '1', 2024, 94.3),
(111, 6, 7, 14, 0, '2', 2024, 67.6),
(112, 7, 1, 14, 1, '3', 2024, 89.2),
(113, 1, 2, 15, 1, '1', 2024, 85.6),
(114, 2, 3, 15, 0, '2', 2024, 71.8),
(115, 3, 5, 15, 1, '3', 2024, 90.7),
(116, 4, 6, 16, 1, '1', 2024, 93.0),
(117, 5, 7, 16, 0, '2', 2024, 69.2),
(118, 6, 1, 16, 1, '3', 2024, 88.4),
(119, 7, 2, 17, 1, '1', 2024, 92.1),
(120, 1, 3, 17, 0, '2', 2024, 73.5);

-- Datos de prueba para rubricas
INSERT INTO [dbo].[rubricas]
           ([Id], [IdSO], [IdProfesor], [IdAsignatura], [IdEstado], [IdMetodoEvaluacion], [FechaCompletado], 
            [UltimaEdicion], [CantEstudiantes], [Año], [Periodo], [Seccion], [Comentario], [Problematica], 
            [Solucion], [Evidencia], [EvaluacionesFormativas], [Estrategias]) 
VALUES
(1, 1, 3, 5, 1, 2, GETDATE(), NULL, 30, 2024, 'Primer Semestre', 'A', 'Buena participación', 
 'Dificultades con conceptos avanzados', 'Revisión grupal', 'Evidencia en informe final', 
 'Pruebas escritas', 'Tutorías individuales'),

(2, 2, 4, 8, 2, 3, GETDATE(), NULL, 25, 2024, 'Primer Semestre', 'B', 'Evaluación regular', 
 'Problemas con la metodología de trabajo', 'Implementación de ejercicios guiados', 
 'Archivos PDF con análisis', 'Exámenes parciales', 'Uso de software especializado'),

(3, 3, 5, 12, 3, 1, GETDATE(), NULL, 28, 2024, 'Segundo Semestre', 'C', 'Resultados mixtos', 
 'Poca participación en debates', 'Integración de foros de discusión', 
 'Videos de presentaciones', 'Autoevaluaciones', 'Aprendizaje basado en proyectos'),

(4, 4, 6, 15, 1, 4, GETDATE(), NULL, 32, 2024, 'Primer Semestre', 'D', 'Dificultad en aplicación práctica', 
 'Falta de experiencia con herramientas de desarrollo', 'Laboratorios adicionales', 
 'Reportes de pruebas', 'Pruebas en línea', 'Trabajo en equipo'),

(5, 5, 7, 18, 4, 2, GETDATE(), NULL, 20, 2024, 'Segundo Semestre', 'E', 'Avance satisfactorio', 
 'Requieren más material de apoyo', 'Entrega de material complementario', 
 'Resúmenes escritos', 'Evaluaciones orales', 'Casos de estudio'),

(6, 6, 3, 20, 1, 3, GETDATE(), NULL, 35, 2024, 'Primer Semestre', 'F', 'Excelente desempeño', 
 'Algunos estudiantes no completan tareas', 'Refuerzo con sesiones de repaso', 
 'Videos explicativos', 'Cuestionarios en línea', 'Clases interactivas'),

(7, 7, 4, 21, 3, 1, GETDATE(), NULL, 27, 2024, 'Tercer Semestre', 'G', 'Desempeño variado', 
 'Falta de estructura en respuestas escritas', 'Prácticas de redacción técnica', 
 'Ejercicios de escritura', 'Corrección de ensayos', 'Revisión por pares'),

(8, 1, 5, 23, 2, 4, GETDATE(), NULL, 33, 2024, 'Cuarto Semestre', 'H', 'Alto interés en la asignatura', 
 'Necesidad de mayor práctica en código', 'Programación en vivo', 
 'Capturas de código', 'Proyectos individuales', 'Desarrollo de software en equipo'),

(9, 2, 6, 25, 1, 2, GETDATE(), NULL, 29, 2024, 'Primer Semestre', 'I', 'Falta de enfoque en los detalles', 
 'Dificultad en depuración de código', 'Ejercicios específicos de depuración', 
 'Capturas de pantalla', 'Simulaciones', 'Tareas prácticas'),

(10, 3, 7, 26, 3, 3, GETDATE(), NULL, 22, 2024, 'Segundo Semestre', 'J', 'Falta de dominio de ciertos temas', 
 'Falta de práctica con algoritmos recursivos', 'Ejercicios guiados', 
 'Códigos de muestra', 'Exámenes abiertos', 'Uso de herramientas online');

-- Datos de prueba para resumen
INSERT INTO [dbo].[resumen] ([Id_PI], [Id_Rubrica], [CantDesarrollo], [CantExperto], [CantPrincipiante], [CantSatisfactorio]) 
VALUES
(1, 1, 10, 5, 3, 7),
(2, 2, 8, 6, 4, 5),
(3, 3, 12, 4, 2, 9),
(4, 4, 15, 2, 3, 10),
(5, 5, 9, 7, 6, 4),
(6, 6, 11, 8, 5, 6),
(7, 7, 14, 3, 2, 8),
(1, 8, 7, 9, 4, 6),
(2, 9, 10, 6, 3, 7),
(3, 10, 8, 7, 5, 5),
(4, 1, 12, 5, 4, 9),
(5, 2, 14, 3, 2, 8),
(6, 3, 9, 6, 7, 5),
(7, 4, 10, 5, 6, 7),
(1, 5, 11, 4, 3, 8),
(2, 6, 12, 7, 5, 9),
(3, 7, 15, 3, 2, 10),
(4, 8, 7, 8, 5, 6),
(5, 9, 9, 6, 4, 7),
(6, 10, 13, 4, 2, 9);

-- Datos de prueba para mapa_competencias
INSERT INTO [dbo].[mapa_competencias] ([Id_Asignatura], [Id_Competencia], [Id_Estado]) 
VALUES
(1, 1, 11),
(2, 2, 11),
(3, 3, 11),
(4, 4, 11),
(5, 5, 11),
(6, 6, 11),
(7, 7, 11),
(8, 1, 11),
(9, 2, 11),
(10, 3, 11),
(11, 4, 11),
(12, 5, 11),
(13, 6, 11),
(14, 7, 11),
(15, 1, 11),
(16, 2, 11),
(17, 3, 11),
(18, 4, 11),
(19, 5, 11),
(20, 6, 11),
(21, 7, 11),
(22, 1, 11),
(23, 2, 11),
(24, 3, 11),
(25, 4, 11),
(26, 5, 11),
(27, 6, 11),
(28, 7, 11);


-- Datos de prueba para action_plan
INSERT INTO [dbo].[action_plan] 
([Id], [Id_Desempeno], [FechaCreacion], [UltimaEdicion], [Id_Rubrica], [Descripcion], [Id_Estado]) 
VALUES
(1, 1, GETDATE(), NULL, 1, 'Mejorar la calidad de la enseñanza en Ingeniería', 1),
(2, 2, GETDATE(), NULL, 2, 'Implementar estrategias para el desarrollo de software', 2);

-- Datos de prueba para aula
INSERT INTO [dbo].[aula] 
([Id], [Descripcion], [FechaCreacion], [UltimaEdicion], [Id_Edificio], [Id_Estado]) 
VALUES
(1, 'Laboratorio de Software', GETDATE(), NULL, 1, 1),
(2, 'Aula de Matemáticas', GETDATE(), NULL, 2, 1);

-- Datos de prueba para contacto
INSERT INTO [dbo].[contacto] 
([Id], [NumeroContacto], [Id_Usuario]) 
VALUES
(1, '809-555-1234', 1),
(2, '809-555-5678', 2);

-- Datos de prueba para edificios
INSERT INTO [dbo].[edificios] 
([Id], [Id_Area], [Nombre], [FechaCreacion], [UltimaEdicion], [Acron], [Id_Estado], [Ubicacion]) 
VALUES
(1, 1, 'Edificio de Ingeniería', GETDATE(), NULL, 'ING', 1, 'Campus Norte'),
(2, 2, 'Edificio de Ciencias', GETDATE(), NULL, 'SCI', 1, 'Campus Central');

-- Datos de prueba para evidencia
INSERT INTO [dbo].[evidencia] 
([Id], [Id_Rubrica], [Nombre], [Ruta], [FechaCreacion]) 
VALUES
(1, 1, 'Reporte de Evaluación', '/evidencias/reporte1.pdf', GETDATE()),
(2, 2, 'Presentación Final', '/evidencias/presentacion.pdf', GETDATE());

-- Datos de prueba para historial_incumplimiento
INSERT INTO [dbo].[historial_incumplimiento] 
([Id], [Descripcion], [Fecha], [Id_Usuario]) 
VALUES
(1, 'Entrega tardía de informe', GETDATE(), 1),
(2, 'No cumplimiento de requisitos de evaluación', GETDATE(), 2);

-- Datos de prueba para informe
INSERT INTO [dbo].[informe] 
([Id], [Ruta], [FechaCreacion], [Nombre], [Tipo_Id], [Carrera_Id], [Año], [Trimestre], [Periodo]) 
VALUES
(1, '/informes/informe1.pdf', GETDATE(), 'Informe Anual Ingeniería', 1, 1, 2024, '1', 'Enero-Marzo'),
(2, '/informes/informe2.pdf', GETDATE(), 'Informe Evaluación Software', 2, 2, 2024, '2', 'Abril-Junio');

-- Datos de prueba para inventario
INSERT INTO [dbo].[inventario] 
([Id], [Descripcion], [FechaCreacion], [UltimaEdicion], [Id_Estado]) 
VALUES
(1, 'Computadoras portátiles', GETDATE(), NULL, 1),
(2, 'Proyectores multimedia', GETDATE(), NULL, 1);

-- Datos de prueba para objeto_aula
INSERT INTO [dbo].[objeto_aula] 
([Id_Objeto], [Id_Aula], [Cantidad]) 
VALUES
(1, 1, 10),
(2, 2, 5);

-- Datos de prueba para profesor_carrera
INSERT INTO [dbo].[profesor_carrera] 
([Profesor_Id], [Carrera_Id]) 
VALUES
(1, 1),
(2, 2);

-- Datos de prueba para tarea
INSERT INTO [dbo].[tarea] 
([Id], [Id_ActionPlan], [Id_Auxiliar], [Id_EstadoTarea], [FechaCreacion], [UltimaEdicion], [Descripcion]) 
VALUES
(1, 1, 1, 1, GETDATE(), NULL, 'Revisión de informes de desempeño'),
(2, 2, 2, 2, GETDATE(), NULL, 'Implementación de mejoras en evaluación');

-- Datos de prueba para tipo_informe
INSERT INTO [dbo].[tipo_informe] 
([Id], [Descripcion]) 
VALUES
(1, 'Informe Semestral'),
(2, 'Reporte Anual');

-- Datos de prueba para usuario
INSERT INTO [dbo].[usuario] 
([Id], [Username], [Email], [HashedPassword], [Salt], [FechaCreacion], [UltimaEdicion], [FechaEliminacion], [Nombre], [Apellido], [Foto], [CV], [IdSO], [IdEstado], [IdArea], [IdRol], [RolId]) 
VALUES
(1, 'jdoe', 'jdoe@email.com', 'hashedpassword123', 'salt123', GETDATE(), NULL, NULL, 'John', 'Doe', '/images/jdoe.jpg', '/cv/jdoe.pdf', 1, 1, 1, 1, 1),
(2, 'asmith', 'asmith@email.com', 'hashedpassword456', 'salt456', GETDATE(), NULL, NULL, 'Alice', 'Smith', '/images/asmith.jpg', '/cv/asmith.pdf', 2, 1, 2, 2, 2),
(3, 'prof_01', 'prof01@email.com', 'pass01', 'salt01', GETDATE(), NULL, NULL, 'Carlos', 'Gómez', '/images/prof01.jpg', '/cv/prof01.pdf', 1, 1, 1, 1, 1),
(4, 'prof_02', 'prof02@email.com', 'pass02', 'salt02', GETDATE(), NULL, NULL, 'Laura', 'Fernández', '/images/prof02.jpg', '/cv/prof02.pdf', 2, 1, 2, 1, 1),
(5, 'prof_03', 'prof03@email.com', 'pass03', 'salt03', GETDATE(), NULL, NULL, 'María', 'López', '/images/prof03.jpg', '/cv/prof03.pdf', 3, 1, 3, 1, 1),
(6, 'prof_04', 'prof04@email.com', 'pass04', 'salt04', GETDATE(), NULL, NULL, 'Javier', 'Pérez', '/images/prof04.jpg', '/cv/prof04.pdf', 4, 1, 4, 1, 1),
(7, 'prof_05', 'prof05@email.com', 'pass05', 'salt05', GETDATE(), NULL, NULL, 'Ana', 'Ramírez', '/images/prof05.jpg', '/cv/prof05.pdf', 5, 1, 5, 1, 1);


-- Datos de prueba para ConfiguracionEvaluaciones
INSERT INTO [dbo].[ConfiguracionEvaluaciones] 
([Id], [Descripcion], [FechaInicio], [FechaCierre], [Id_Estado]) 
VALUES
(1, 'Evaluación de Medio Término', '2024-03-01', '2024-03-31', 1),
(2, 'Evaluación Final', '2024-06-01', '2024-06-30', 1);


-- Datos de prueba para carrera_rubrica 
INSERT INTO [dbo].[carrera_rubrica] 
([Id_Rubrica], [Id_Carrera]) 
VALUES
(1, 1),
(2, 2);
